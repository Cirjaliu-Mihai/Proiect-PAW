using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Repositories.Interfaces;

namespace BarberSalon.Repositories
{
    public class PortofolioRepository : RepositoryBase<PortofolioItem>, IPortofolioRepository
    {
        public PortofolioRepository(BarberSalonContext barberContext) : base(barberContext)
        {
        }
    }
}