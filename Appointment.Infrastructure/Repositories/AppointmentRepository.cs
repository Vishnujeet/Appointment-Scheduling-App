using Appointment.Infrastructure.Interface;
using Appointment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Appointment.Common.DTOs;
using Appointment.Domain.Entities;

namespace Appointment.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;

        public AppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        //commented this approach because of the error "Npgsql.PostgresException (0x80004005): 42883: operator does not exist: character varying[] @> text[]"
       
        //public async Task<IEnumerable<AvailableSlotsDto>> GetAvailableSlots(DateTime date, List<string> products, string language, string rating)
        //{
        //    // Fetch matching sales managers based on criteria
        //    //var matchingManagers = await _context.SalesManagers
        //    //    .Where(sm =>
        //    //        sm.Languages.Any(l => l == language) &&  // ✅ Proper array filtering
        //    //        sm.Products.Any(p => products.Contains(p)) &&
        //    //        sm.CustomerRatings.Any(r => r == rating))
        //    //    .Select(sm => sm.Id)
        //    //    .ToListAsync();
        //    var allManagers = await _context.SalesManagers.ToListAsync(); // Get all managers first

        //    if (!matchingManagers.Any())
        //        return new List<AvailableSlotsDto>(); // No matching sales managers

        //    //  Fetch slots for matching managers on the specified date
        //    var allSlots = await _context.Slots
        //        .Where(s => matchingManagers.Contains(s.SalesManagerId) && s.StartDate.Date == date.Date)
        //        .ToListAsync();

        //    //  Filter out booked and overlapping slots
        //    var availableSlots = allSlots
        //        .Where(slot =>
        //            !slot.Booked &&
        //            !allSlots.Any(s =>
        //                s.SalesManagerId == slot.SalesManagerId &&
        //                s.Booked &&
        //                IsOverlapping(s.StartDate, s.EndDate, slot.StartDate, slot.EndDate)))
        //        .ToList();

        //    //  Group slots by StartDate to count available slots
        //    var groupedSlots = availableSlots
        //        .GroupBy(s => s.StartDate)
        //        .Select(g => new AvailableSlotsDto
        //        {
        //            StartDate = g.Key,
        //            AvailableCount = g.Count()
        //        })
        //        .OrderBy(s => s.StartDate)
        //        .ToList();

        //    return groupedSlots;
        //}

        public async Task<IEnumerable<AvailableSlotsDto>> GetAvailableSlots(DateTime date, List<string> products, string language, string rating)
        {
            //Fetch all sales managers first
            var allManagers = await _context.SalesManagers.ToListAsync();

            var matchingManagers = new List<int>(); // Store matching manager IDs

            foreach (var manager in allManagers)
            {
                bool hasLanguage = manager.Languages.Contains(language);
                bool hasProduct = manager.Products.Any(p => products.Contains(p));
                bool hasRating = manager.CustomerRatings.Contains(rating);

                if (hasLanguage && hasProduct && hasRating)
                {
                    matchingManagers.Add(manager.Id);
                }
            }

            if (!matchingManagers.Any())
                return new List<AvailableSlotsDto>(); // No matching sales managers

            //Fetch slots for matching managers on the specified date
            var allSlots = await _context.Slots
                .Where(s => matchingManagers.Contains(s.SalesManagerId) && s.StartDate.Date == date.Date)
                .ToListAsync();

            // Filter out booked slots
            var availableSlots = allSlots
                .Where(slot =>
                    !slot.Booked &&
                    !allSlots.Any(s =>
                        s.SalesManagerId == slot.SalesManagerId &&
                        s.Booked &&
                        IsOverlapping(s.StartDate, s.EndDate, slot.StartDate, slot.EndDate)))
                .ToList();


            //Group by StartDate and count available slots
            var groupedSlots = availableSlots
                .GroupBy(s => s.StartDate)
                .Select(g => new AvailableSlotsDto
                {
                    StartDate = g.Key,
                    AvailableCount = g.Count()
                })
                .OrderBy(s => s.StartDate)
                .ToList();

            return groupedSlots;
        }

        //Overlapping Function
        private bool IsOverlapping(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1 < end2 && start2 < end1; 
        }

    }
}
