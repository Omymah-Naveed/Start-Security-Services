using System;
using System.Collections.Generic;

namespace Star_Security_Service.Models
{
    public class BookingViewModel
    {
        public string Name { get; set; } = null!; 
        public string Email { get; set; } = null!; 
        public int EmployeeId { get; set; }        
        public int ServiceId { get; set; }       
        public DateTime BookingDatetime { get; set; } 

        public List<MannedGuarding> Services { get; set; } = new List<MannedGuarding>();
        public List<EmployeeInformation> Emp { get; set; }

    }
}
