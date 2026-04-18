using System;
using System.Collections.Generic;

namespace Star_Security_Service.Models
{
    public partial class EmployeeService
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? ServiceId { get; set; }

        public virtual EmployeeInformation? Employee { get; set; }
        public virtual MannedGuarding? Service { get; set; }
    }
}
