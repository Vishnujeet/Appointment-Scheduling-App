using Appointment.Common.DTOs;

namespace Appointment.Infrastructure.Interface
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<AvailableSlotsDto>> GetAvailableSlots(DateTime date, List<string> products, string language, string rating);
    }
}
