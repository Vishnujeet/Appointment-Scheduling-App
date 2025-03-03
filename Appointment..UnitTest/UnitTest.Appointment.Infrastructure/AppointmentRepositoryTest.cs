using Appointment.Common.DTOs;
using Appointment.Domain.Entities;
using Moq;
using Xunit;
using Appointment.Core.Services;
using Appointment.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.Tests.Repositories
{
    public class AppointmentRepositoryTest
    {
        private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
        private readonly AppointmentService _service;

        public AppointmentRepositoryTest()
        {
            _mockAppointmentRepository = new Mock<IAppointmentRepository>();
            _service = new AppointmentService(_mockAppointmentRepository.Object);
        }

        [Fact]
        public async Task GetAvailableSlots_ShouldReturnEmpty_WhenNoMatchingManagers()
        {
            // Arrange: No matching sales managers
            _mockAppointmentRepository
                .Setup(repo => repo.GetAvailableSlots(It.IsAny<DateTime>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<AvailableSlotsDto>());

            // Act
            var result = await _service.GetAvailableSlots(DateTime.UtcNow.Date, new List<string> { "SolarPanels" }, "German", "Gold");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAvailableSlots_ShouldReturnAvailableSlots()
        {
            // Arrange
            var date = new DateTime(2024, 05, 03);
            var products = new List<string> { "SolarPanels", "Heatpumps" };
            var language = "German";
            var rating = "Gold";

            var expectedSlots = new List<AvailableSlotsDto>
            {
                new AvailableSlotsDto { StartDate = date.AddHours(10).AddMinutes(30), AvailableCount = 1 },
                new AvailableSlotsDto { StartDate = date.AddHours(12), AvailableCount = 1 }
            };

            _mockAppointmentRepository
                .Setup(repo => repo.GetAvailableSlots(date, products, language, rating))
                .ReturnsAsync(expectedSlots);

            // Act
            var result = await _service.GetAvailableSlots(date, products, language, rating);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, slot => slot.StartDate == date.AddHours(10).AddMinutes(30) && slot.AvailableCount == 1);
            Assert.Contains(result, slot => slot.StartDate == date.AddHours(12) && slot.AvailableCount == 1);
        }

        [Fact]
        public async Task GetAvailableSlots_ShouldExcludeBookedSlots()
        {
            // Arrange
            var date = new DateTime(2024, 05, 03);
            var products = new List<string> { "SolarPanels" };
            var language = "German";
            var rating = "Gold";

            _mockAppointmentRepository
                .Setup(repo => repo.GetAvailableSlots(date, products, language, rating))
                .ReturnsAsync(new List<AvailableSlotsDto>()); // No available slots

            // Act
            var result = await _service.GetAvailableSlots(date, products, language, rating);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAvailableSlots_ShouldHandleOverlappingSlots()
        {
            // Arrange
            var date = new DateTime(2024, 05, 03);
            var products = new List<string> { "SolarPanels" };
            var language = "German";
            var rating = "Gold";

            var expectedSlots = new List<AvailableSlotsDto>
            {
                new AvailableSlotsDto { StartDate = date.AddHours(10).AddMinutes(30), AvailableCount = 1 },
                new AvailableSlotsDto { StartDate = date.AddHours(10).AddMinutes(45), AvailableCount = 1 } 
            };

            _mockAppointmentRepository
                .Setup(repo => repo.GetAvailableSlots(date, products, language, rating))
                .ReturnsAsync(expectedSlots);

            // Act
            var result = await _service.GetAvailableSlots(date, products, language, rating);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, slot => slot.StartDate == date.AddHours(10).AddMinutes(30));
            Assert.Contains(result, slot => slot.StartDate == date.AddHours(10).AddMinutes(45));
        }
    }
}
