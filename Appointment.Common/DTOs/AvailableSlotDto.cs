using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Common.DTOs
{
    public class AvailableSlotsDto
    {
        public int AvailableCount { get; set; }
        public DateTime StartDate { get; set; }
    }
}
