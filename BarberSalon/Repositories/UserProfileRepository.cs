using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Repositories.Interfaces;

namespace BarberSalon.Repositories
{
    public class UserProfileRepository : RepositoryBase<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(BarberSalonContext barberContext) : base(barberContext)
        {
        }
    }
}
