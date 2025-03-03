using Appointment.Common.DTOs;
using Appointment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Infrastructure.Data
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options) { }

        public DbSet<SalesManager> SalesManagers { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<AvailableSlotsDto> AvailableSlots { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesManager>().ToTable("sales_managers");
            modelBuilder.Entity<Slot>().ToTable("slots");
            modelBuilder.Entity<AvailableSlotsDto>().HasNoKey(); // This marks the entity as keyless
        }

    }
}
