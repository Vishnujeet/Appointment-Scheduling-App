﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Appointment.Domain.Entities
{
    [Table("slots")]
    public class Slot
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("booked")]
        public bool Booked { get; set; }

        [Column("sales_manager_id")]
        public int SalesManagerId { get; set; }

        [ForeignKey("SalesManagerId")]
        public SalesManager SalesManager { get; set; } = null!;
    }
}
