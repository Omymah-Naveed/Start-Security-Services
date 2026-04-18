namespace Star_Security_Service.Models
{
    public class MannedGuardingModelView
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile Image { get; set; }

        public string? Items { get; set; }

        public List<MannedGuarding> MannedGuardingList { get; set; }
        public List<Testimonial> Tes { get; set; } = new List<Testimonial>();


    }
}
