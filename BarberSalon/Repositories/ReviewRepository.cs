using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Repositories.Interfaces;

namespace BarberSalon.Repositories
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewsRepository
    {
        public ReviewRepository(BarberSalonContext barberContext) : base(barberContext)
        {
        }
    }
}