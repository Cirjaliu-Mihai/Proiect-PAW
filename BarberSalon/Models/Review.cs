namespace BarberSalon.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public required Service Service { get; set; }
        public int WorkerId { get; set; }
        public required Worker Worker { get; set; }
        public int Rating { get; set; }
        public required string Comment { get; set; }
    }
}
