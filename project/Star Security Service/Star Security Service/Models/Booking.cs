using System;
using System.Collections.Generic;

namespace Star_Security_Service.Models
{
    public partial class Booking
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public DateTime BookingDatetime { get; set; }

        public virtual EmployeeInformation Employee { get; set; } = null!;
        public virtual MannedGuarding Service { get; set; } = null!;
    }
}
