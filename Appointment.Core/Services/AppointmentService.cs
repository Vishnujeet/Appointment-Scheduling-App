using Appointment.Common.DTOs;
using Appointment.Core.Interfaces;
using Appointment.Infrastructure.Interface;

namespace Appointment.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;

        public AppointmentService(IAppointmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AvailableSlotsDto>> GetAvailableSlots(DateTime date, List<string> products, string language, string rating)
        {
            if (products == null || !products.Any())
            {
                throw new ArgumentException("Product list cannot be empty.", nameof(products));
            }

            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            try
            {
                var slots = await _repository.GetAvailableSlots(date, products, language, rating);
                return slots ?? new List<AvailableSlotsDto>(); 
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while fetching available slots.", ex);
            }
        }
    }
}
