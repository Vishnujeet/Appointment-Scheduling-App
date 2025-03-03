using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Appointment.API.Controllers;
using Appointment.Core.Interfaces;
using Appointment.API.DTO;
using Appointment.Common.DTOs;

namespace UnitTest.Appointment.Api
{
    public class AppointmentControllerTests
    {
        private readonly Mock<IAppointmentService> _mockService;
        private readonly AppointmentController _controller;

        public AppointmentControllerTests()
        {
            _mockService = new Mock<IAppointmentService>();
            _controller = new AppointmentController(_mockService.Object);
        }

        [Fact]
        public async Task GetAvailableSlots_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new SlotRequest
            {
                Date = "2025-03-10",
                Products = new List<string> { "ProductA" },
                Language = "English",
                Rating = "5"
            };

            var mockSlots = new List<AvailableSlotsDto>
            {
                new AvailableSlotsDto { StartDate = DateTime.UtcNow, AvailableCount = 2 }
            };

            _mockService
                .Setup(s => s.GetAvailableSlots(It.IsAny<DateTime>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mockSlots);

            // Act
            var result = await _controller.GetAvailableSlots(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedSlots = Assert.IsType<List<AvailableSlotsDto>>(okResult.Value);
            Assert.Single(returnedSlots);
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
            var result = await _controller.GetAvailableSlots(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid date format.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAvailableSlots_EmptyProducts_ReturnsBadRequest()
        {
            // Arrange
            var request = new SlotRequest
            {
                Date = "2025-03-10",
                Products = new List<string>(), // Empty list
                Language = "English",
                Rating = "5"
            };

            // Act
            var result = await _controller.GetAvailableSlots(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid request data.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetAvailableSlots_NoAvailableSlots_ReturnsNoContent()
        {
            // Arrange
            var request = new SlotRequest
            {
                Date = "2025-03-10",
                Products = new List<string> { "ProductA" },
                Language = "English",
                Rating = "5"
            };

            _mockService
                .Setup(s => s.GetAvailableSlots(It.IsAny<DateTime>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new List<AvailableSlotsDto>());

            // Act
            var result = await _controller.GetAvailableSlots(request);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task GetAvailableSlots_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var request = new SlotRequest
            {
                Date = "2025-03-10",
                Products = new List<string> { "ProductA" },
                Language = "English",
                Rating = "5"
            };

            _mockService
                .Setup(s => s.GetAvailableSlots(It.IsAny<DateTime>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAvailableSlots(request);

            // Assert
            var internalServerError = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, internalServerError.StatusCode);
            Assert.Equal("An error occurred while fetching available slots.", internalServerError.Value);
        }
    }
}
