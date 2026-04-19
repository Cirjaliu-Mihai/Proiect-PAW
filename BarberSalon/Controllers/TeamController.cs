using Microsoft.AspNetCore.Mvc;
using BarberSalon.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Controllers
{
    public class TeamController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public TeamController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IActionResult> Index()
        {
            var workers = await _repositoryWrapper.WorkerRepository
                .FindAll()
                .ToListAsync();
            return View(workers);
        }
    }
}
