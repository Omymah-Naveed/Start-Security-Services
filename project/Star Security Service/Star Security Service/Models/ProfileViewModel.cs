namespace Star_Security_Service.Models
{
    public class ProfileViewModel
    {
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<EmployeeInformation> EmployeeInfo { get; set; } = new List<EmployeeInformation>();
        public List<EmployeeService> EmployeeSer { get; set; } = new List<EmployeeService>();
    }
}
