using BarberSalon.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AccountController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IActionResult> Profile()
        {
            // TODO: replace "Andrei Popescu" with User.Identity.Name once auth is implemented
            var currentUser = "Andrei Popescu";
            var reviews = await _repositoryWrapper.ReviewsRepository
                .FindByCondition(r => r.Author == currentUser)
                .Include(r => r.Service)
                .Include(r => r.Worker)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            ViewBag.MyReviews = reviews;
            return View();
        }
    }
}