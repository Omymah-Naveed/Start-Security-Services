using System;
using System.Collections.Generic;

namespace Star_Security_Service.Models
{
    public partial class Contact
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phonenumber { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
    }
}
