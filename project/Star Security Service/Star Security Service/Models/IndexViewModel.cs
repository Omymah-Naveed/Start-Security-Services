namespace Star_Security_Service.Models
{
    public class IndexViewModel
    {
        public List<MannedGuarding> Man { get; set; } = new List<MannedGuarding>();
        public List<Network> Net { get; set; } = new List<Network>();
        public List<Testimonial> Tes { get; set; } = new List<Testimonial>();


    }
}
