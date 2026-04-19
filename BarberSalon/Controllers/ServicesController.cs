using Microsoft.AspNetCore.Mvc;
using BarberSalon.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ServicesController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _repositoryWrapper.ServiceRepository
                .FindAll()
                .ToListAsync();
            return View(services);
        }
    }
}
