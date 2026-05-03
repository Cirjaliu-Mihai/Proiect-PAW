using Microsoft.AspNetCore.Mvc;
using BarberSalon.Repositories.Interfaces;
using BarberSalon.Models;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Service service)
        {
            if (!ModelState.IsValid)
                return View(service);

            _repositoryWrapper.ServiceRepository.Create(service);
            _repositoryWrapper.save();
            TempData["Success"] = "Serviciul a fost adăugat cu succes!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var service = await _repositoryWrapper.ServiceRepository
                .FindByCondition(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (service == null)
                return NotFound();

            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Service service)
        {
            if (id != service.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(service);

            _repositoryWrapper.ServiceRepository.Update(service);
            _repositoryWrapper.save();
            TempData["Success"] = "Serviciul a fost actualizat cu succes!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await _repositoryWrapper.ServiceRepository
                .FindByCondition(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (service == null)
                return NotFound();

            _repositoryWrapper.ServiceRepository.Delete(service);
            _repositoryWrapper.save();
            TempData["Success"] = "Serviciul a fost șters cu succes!";
            return RedirectToAction(nameof(Index));
        }
    }
}
