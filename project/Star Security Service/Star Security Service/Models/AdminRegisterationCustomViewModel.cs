using System.Data;

namespace Star_Security_Service.Models
{
    public class AdminRegisterationCustomViewModel
    {
        public IEnumerable<AdminRegisterationRole>? RoleList { get; set; } 
        public AdminRegisteration registrationFormData { get; set; }
    }
}
