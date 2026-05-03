using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarberSalon.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public required int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string Phone { get; set; } = "";
        public DateTime MemberSince { get; set; } = new DateTime();
        public string PhotoPath { get; set; } = "/assets/images/poza-profil.png";
    }
}
