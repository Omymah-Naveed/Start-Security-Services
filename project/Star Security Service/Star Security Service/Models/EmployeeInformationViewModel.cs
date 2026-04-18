namespace Star_Security_Service.Models
{
    public class EmployeeInformationViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phonenumber { get; set; }
        public string? Qualification { get; set; }
        public int? ServiceId { get; set; }
        public string? Grade { get; set; }
        public string? Client { get; set; }
        public string? Achievements { get; set; }
        public string Email { get; set; }
        public List<MannedGuarding>? Services { get; set; }
        public EmployeeInformation employeeInformation { get; set; }
    }
}
