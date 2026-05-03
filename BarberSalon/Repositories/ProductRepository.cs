using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Repositories.Interfaces;

namespace BarberSalon.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(BarberSalonContext barberSalonContext) : base(barberSalonContext)
        {
        }
    }
}
