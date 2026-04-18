using System;
using System.Collections.Generic;

namespace Star_Security_Service.Models
{
    public partial class AdminRegisterationRole
    {
        public AdminRegisterationRole()
        {
            AdminRegisterations = new HashSet<AdminRegisteration>();
        }

        public int Id { get; set; }
        public string? Role { get; set; }

        public virtual ICollection<AdminRegisteration> AdminRegisterations { get; set; }
    }
}
