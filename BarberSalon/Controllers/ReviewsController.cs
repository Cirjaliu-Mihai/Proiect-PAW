using BarberSalon.Repositories.Interfaces;
using BarberSalon.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IReviewService _reviewService;

        public ReviewsController(IRepositoryWrapper repositoryWrapper, IReviewService reviewService)
        {
            _repositoryWrapper = repositoryWrapper;
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return View(reviews);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Services = new SelectList(
                _repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name");
            ViewBag.Workers = new SelectList(
                _repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name");
            ViewBag.CurrentUser = "Andrei Popescu";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int serviceId, int workerId, int rating, string comment, string author)
        {
            try
            {
                await _reviewService.CreateReviewAsync(serviceId, workerId, rating, comment, author);
                TempData["Success"] = "Recenzia ta a fost adăugată cu succes!";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            ViewBag.Services = new SelectList(
                _repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name", serviceId);
            ViewBag.Workers = new SelectList(
                _repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name", workerId);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null) return NotFound();

            ViewBag.Services = new SelectList(
                _repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name", review.ServiceId);
            ViewBag.Workers = new SelectList(
                _repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name", review.WorkerId);
            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int serviceId, int workerId, int rating, string comment)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null) return NotFound();

            if (serviceId == 0 || workerId == 0 || rating < 1 || rating > 5 || string.IsNullOrWhiteSpace(comment))
            {
                ModelState.AddModelError(string.Empty, "Completează toate câmpurile corect.");
                ViewBag.Services = new SelectList(
                    _repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name", serviceId);
                ViewBag.Workers = new SelectList(
                    _repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name", workerId);
                return View(review);
            }

            review.ServiceId = serviceId;
            review.WorkerId = workerId;
            review.Rating = rating;
            review.Comment = comment;
            review.Service = null;
            review.Worker = null;

            _repositoryWrapper.ReviewsRepository.Update(review);
            _repositoryWrapper.save();

            TempData["Success"] = "Recenzia a fost actualizată cu succes!";
            return RedirectToAction("Profile", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            TempData["Success"] = "Recenzia a fost ștearsă.";
            return RedirectToAction("Profile", "Account");
        }
    }
}
