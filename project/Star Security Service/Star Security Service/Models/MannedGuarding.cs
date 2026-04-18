using System;
using System.Collections.Generic;

namespace Star_Security_Service.Models
{
    public partial class MannedGuarding
    {
        public MannedGuarding()
        {
            Bookings = new HashSet<Booking>();
            EmployeeInformations = new HashSet<EmployeeInformation>();
            EmployeeServices = new HashSet<EmployeeService>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? Items { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<EmployeeInformation> EmployeeInformations { get; set; }
        public virtual ICollection<EmployeeService> EmployeeServices { get; set; }
    }
}
