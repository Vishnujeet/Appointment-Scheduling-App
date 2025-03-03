using Microsoft.AspNetCore.Mvc;
using Appointment.Core.Interfaces;
using Appointment.API.DTO;
using Appointment.Common.DTOs;

namespace Appointment.API.Controllers
{
    [Route("calendar")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("query")]
        public async Task<ActionResult<IEnumerable<AvailableSlotsDto>>> GetAvailableSlots([FromBody] SlotRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Date) || request.Products == null || !request.Products.Any())
            {
                return BadRequest("Invalid request data.");
            }

            if (!DateTime.TryParse(request.Date, out DateTime parsedDate))
            {
                return BadRequest("Invalid date format.");
            }

            try
            {
                var slots = await _appointmentService.GetAvailableSlots(parsedDate, request.Products, request.Language, request.Rating);

                if (slots == null || !slots.Any())
                {
                    return NoContent();
                }

                return Ok(slots);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching available slots.");
            }
        }
    }
}
