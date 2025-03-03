using Appointment.Common.DTOs;

namespace Appointment.Core.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AvailableSlotsDto>> GetAvailableSlots(DateTime date, List<string> products, string language, string rating);
    }
}
