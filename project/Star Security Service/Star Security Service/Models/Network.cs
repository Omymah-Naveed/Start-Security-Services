using System;
using System.Collections.Generic;

namespace Star_Security_Service.Models
{
    public partial class Network
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Cell { get; set; }
        public string? Location { get; set; }
        public string? Email { get; set; }
    }
}
