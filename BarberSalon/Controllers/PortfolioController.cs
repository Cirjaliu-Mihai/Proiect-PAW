using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BarberSalon.Services.Interfaces;
using BarberSalon.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public PortfolioController(IPortfolioService portfolioService, IRepositoryWrapper repositoryWrapper)
        {
            _portfolioService = portfolioService;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IActionResult> Index()
        {
            var portfolios = await _portfolioService.GetAllPortfoliosAsync();
            return View(portfolios);
        }

        public async Task<IActionResult> Create()
        {
            var services = await _repositoryWrapper.ServiceRepository.FindAll().ToListAsync();
            var workers = await _repositoryWrapper.WorkerRepository.FindAll().ToListAsync();

            ViewData["ServiceId"] = new SelectList(services, "Id", "Name");
            ViewData["WorkerId"] = new SelectList(workers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int ServiceId, int WorkerId, IFormFile ImageFile)
        {
            try
            {
                if (ImageFile == null || ImageFile.Length == 0)
                {
                    throw new ArgumentException("Please upload an image");
                }

                // Save the image to wwwroot/assets folder
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", fileName);

                // Ensure the assets directory exists
                var dirPath = Path.GetDirectoryName(filePath);
                if (dirPath != null) Directory.CreateDirectory(dirPath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                var imageUrl = $"/assets/{fileName}";
                await _portfolioService.CreatePortfolioAsync(ServiceId, WorkerId, imageUrl);

                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                var services = await _repositoryWrapper.ServiceRepository.FindAll().ToListAsync();
                var workers = await _repositoryWrapper.WorkerRepository.FindAll().ToListAsync();
                ViewData["ServiceId"] = new SelectList(services, "Id", "Name", ServiceId);
                ViewData["WorkerId"] = new SelectList(workers, "Id", "Name", WorkerId);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating portfolio item: " + ex.Message);
                var services = await _repositoryWrapper.ServiceRepository.FindAll().ToListAsync();
                var workers = await _repositoryWrapper.WorkerRepository.FindAll().ToListAsync();
                ViewData["ServiceId"] = new SelectList(services, "Id", "Name", ServiceId);
                ViewData["WorkerId"] = new SelectList(workers, "Id", "Name", WorkerId);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _portfolioService.DeletePortfolioAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
