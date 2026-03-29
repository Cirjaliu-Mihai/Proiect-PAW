using BarberSalon.Models.Enums;

namespace BarberSalon.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ServiceId { get; set; }

        public required Service Service { get; set; }

        public int WorkerId { get; set; }

        public required Worker Worker { get; set; }

        public DateTime AppointmentDate { get; set; }

        public TimeOnly AppointmentTime { get; set; }

        public Status Status { get; set; } = Status.Confirmed;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
