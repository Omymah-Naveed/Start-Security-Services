using System;
using System.Collections.Generic;

namespace Star_Security_Service.Models
{
    public partial class AdminRegisteration
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? Role { get; set; }

        public virtual AdminRegisterationRole? RoleNavigation { get; set; }
    }
}
