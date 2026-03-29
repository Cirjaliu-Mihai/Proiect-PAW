namespace BarberSalon.Models
{
    public class PortofolioItem
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public required Service Service { get; set; }
        public int WorkerId { get; set; }
        public required Worker Worker { get; set; }
        public string? ImageUrl { get; set; }
    }
}
