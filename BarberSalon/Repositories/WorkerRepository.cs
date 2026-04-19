using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Repositories.Interfaces;

namespace BarberSalon.Repositories
{
    public class WorkerRepository : RepositoryBase<Worker>, IWorkerRepository
    {
        public WorkerRepository(BarberSalonContext barberSalonContext) : base(barberSalonContext)
        {
        }
    }
}
