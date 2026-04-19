namespace BarberSalon.Models
{
    public class PortofolioItem
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }
        public string? ImageUrl { get; set; }
    }
}
