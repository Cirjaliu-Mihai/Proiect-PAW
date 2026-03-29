namespace BarberSalon.Models
{
    public class Worker
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Position { get; set; }

        public int PhoneNumber { get; set; }

        public required string Address { get; set; }

        public DateTime HireDate { get; set; }
        
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();


    }
}
