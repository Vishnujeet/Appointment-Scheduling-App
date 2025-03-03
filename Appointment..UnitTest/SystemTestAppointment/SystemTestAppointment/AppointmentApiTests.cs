using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Appointment.API;
using Appointment.API.DTO;
using Appointment.Common.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Appointment.Infrastructure.Data;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Appointment.Domain.Entities;

namespace IntegrationTest.Appointment
{
    public class AppointmentApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public AppointmentApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            { 
                builder.ConfigureServices(services =>
                {
                    //in-memory DB for testing
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppointmentDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<AppointmentDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });

                    using var scope = services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppointmentDbContext>();
                    db.Database.EnsureCreated();

                    SeedTestData(db);
                });
            });

            _client = _factory.CreateClient();
        }

        private void SeedTestData(AppointmentDbContext db)
        {
            db.SalesManagers.Add(new SalesManager
            {
                Id = 1,
                Languages = new List<string> { "English" },
                Products = new List<string> { "ProductA" },
                CustomerRatings = new List<string> { "5" }
            });

            db.Slots.Add(new Slot
            {
                Id = 1,
                SalesManagerId = 1,
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(1).AddHours(1),
                Booked = false
            });

            db.SaveChanges();
        }

        [Fact]
        public async Task GetAvailableSlots_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new SlotRequest
            {
                Date = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd"),
                Products = new List<string> { "ProductA" },
                Language = "English",
                Rating = "5"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/calendar/query", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var slots = await response.Content.ReadFromJsonAsync<List<AvailableSlotsDto>>();
            Assert.NotNull(slots);
            Assert.Single(slots);
        }

        [Fact]
        public async Task GetAvailableSlots_InvalidDate_ReturnsBadRequest()
        {
            // Arrange
            var request = new SlotRequest
            {
                Date = "invalid-date",
                Products = new List<string> { "ProductA" },
                Language = "English",
                Rating = "5"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/calendar/query", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAvailableSlots_NoMatchingSlots_ReturnsNoContent()
        {
            // Arrange
            var request = new SlotRequest
            {
                Date = DateTime.UtcNow.AddDays(30).ToString("yyyy-MM-dd"), 
                Products = new List<string> { "ProductA" },
                Language = "English",
                Rating = "5"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/calendar/query", request);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task GetAvailableSlots_DatabaseFailure_ReturnsInternalServerError()
        {
            // Arrange
            var request = new SlotRequest
            {
                Date = DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-dd"),
                Products = new List<string> { "ProductA" },
                Language = "English",
                Rating = "5"
            };

            // Simulate database failure by disposing of context
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppointmentDbContext>();
                db.Database.EnsureDeleted();
            }

            // Act
            var response = await _client.PostAsJsonAsync("/calendar/query", request);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
