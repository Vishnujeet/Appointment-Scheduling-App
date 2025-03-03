namespace Appointment.API.DTO
{
    public class SlotRequest
    {
        public required string Date { get; set; }
        public required List<string> Products { get; set; }
        public required string Language { get; set; }
        public required string Rating { get; set; }
    }
}
