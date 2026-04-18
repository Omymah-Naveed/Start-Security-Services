using System;
using System.Collections.Generic;

namespace Star_Security_Service.Models
{
    public partial class EmployeeInformation
    {
        public EmployeeInformation()
        {
            Bookings = new HashSet<Booking>();
            EmployeeServices = new HashSet<EmployeeService>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phonenumber { get; set; }
        public string? Qualification { get; set; }
        public int? ServiceId { get; set; }
        public string? Grade { get; set; }
        public string? Client { get; set; }
        public string? Achievements { get; set; }
        public string? Email { get; set; }
        public string? Action { get; set; }

        public virtual MannedGuarding? Service { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<EmployeeService> EmployeeServices { get; set; }
    }
}
