using Moq;
using Xunit;
using Appointment.Core.Services;
using Appointment.Infrastructure.Interface;
using Appointment.Common.DTOs;

namespace UnitTest.Appointment.Core
{
    namespace Appointment.Tests.Services
    {
        public class AppointmentServiceTests
        {
            private readonly Mock<IAppointmentRepository> _mockRepository;
            private readonly AppointmentService _service;

            public AppointmentServiceTests()
            {
                _mockRepository = new Mock<IAppointmentRepository>();
                _service = new AppointmentService(_mockRepository.Object);
            }

            [Fact]
            public async Task GetAvailableSlots_ValidRequest_ReturnsSlots()
            {
                // Arrange
                var requestDate = DateTime.UtcNow;
                var products = new List<string> { "ProductA" };
                var language = "English";
                var rating = "5";

                var mockSlots = new List<AvailableSlotsDto>
            {
                new AvailableSlotsDto { StartDate = requestDate, AvailableCount = 2 }
            };

                _mockRepository
                    .Setup(repo => repo.GetAvailableSlots(requestDate, products, language, rating))
                    .ReturnsAsync(mockSlots);

                // Act
                var result = await _service.GetAvailableSlots(requestDate, products, language, rating);

                // Assert
                Assert.NotNull(result);
                Assert.Single(result);
                Assert.Equal(2, result.First().AvailableCount);
            }

            [Fact]
            public async Task GetAvailableSlots_NoAvailableSlots_ReturnsEmptyList()
            {
                // Arrange
                var requestDate = DateTime.UtcNow;
                var products = new List<string> { "ProductA" };
                var language = "English";
                var rating = "5";

                _mockRepository
                    .Setup(repo => repo.GetAvailableSlots(requestDate, products, language, rating))
                    .ReturnsAsync(new List<AvailableSlotsDto>());

                // Act
                var result = await _service.GetAvailableSlots(requestDate, products, language, rating);

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }

            [Fact]
            public async Task GetAvailableSlots_NullOrEmptyProducts_ThrowsArgumentException()
            {
                // Arrange
                var requestDate = DateTime.UtcNow;
                var emptyProducts = new List<string>();
                var language = "English";
                var rating = "5";

                // Act & Assert
                var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                    _service.GetAvailableSlots(requestDate, emptyProducts, language, rating));

                Assert.Equal("Product list cannot be empty. (Parameter 'products')", exception.Message);
            }

            [Fact]
            public async Task GetAvailableSlots_RepositoryThrowsException_ThrowsApplicationException()
            {
                // Arrange
                var requestDate = DateTime.UtcNow;
                var products = new List<string> { "ProductA" };
                var language = "English";
                var rating = "5";

                _mockRepository
                    .Setup(repo => repo.GetAvailableSlots(requestDate, products, language, rating))
                    .ThrowsAsync(new Exception("Database failure"));

                // Act & Assert
                var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
                    _service.GetAvailableSlots(requestDate, products, language, rating));

                Assert.Equal("An error occurred while fetching available slots.", exception.Message);
            }
        }
    }
}