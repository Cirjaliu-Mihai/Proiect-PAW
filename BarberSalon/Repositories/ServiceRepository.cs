using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Repositories.Interfaces;

namespace BarberSalon.Repositories
{
    public class ServiceRepository : RepositoryBase<Service>, IServiceRepository
    {
        public ServiceRepository(BarberSalonContext barberSalonContext) : base(barberSalonContext)
        {
        }
    }
}
