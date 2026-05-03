using BarberSalon.Models;
using BarberSalon.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BarberSalon.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // TODO: replace with the real authenticated user ID once auth is implemented
        private const int CurrentUserId = 1;

        public AccountController(IRepositoryWrapper repositoryWrapper, IWebHostEnvironment webHostEnvironment)
        {
            _repositoryWrapper = repositoryWrapper;
            _webHostEnvironment = webHostEnvironment;
        }

        private async Task<UserProfile> GetOrCreateProfileAsync()
        {
            var profile = await _repositoryWrapper.UserProfileRepository
                .FindByCondition(p => p.UserId == CurrentUserId)
                .FirstOrDefaultAsync();

            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = CurrentUserId,
                    Name = "Utilizator Nou",
                    Email = "email@exemplu.ro",
                    Phone = "",
                    MemberSince = DateTime.Today
                };
                _repositoryWrapper.UserProfileRepository.Create(profile);
                _repositoryWrapper.save();
            }

            return profile;
        }

        public async Task<IActionResult> Profile()
        {
            var profile = await GetOrCreateProfileAsync();

            var reviews = await _repositoryWrapper.ReviewsRepository
                .FindByCondition(r => r.Author == profile.Name)
                .Include(r => r.Service)
                .Include(r => r.Worker)
                .OrderByDescending(r => r.Id)
                .ToListAsync();

            var allAppointments = await _repositoryWrapper.AppointmentRepository
                .FindAll()
                .ToListAsync();

            ViewBag.MyReviews = reviews;
            ViewBag.TotalAppointments = allAppointments.Count;
            ViewBag.UpcomingAppointments = allAppointments
                .Count(a => a.AppointmentDate >= DateTime.Today && a.Status == Models.Enums.Status.Confirmed);

            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var profile = await GetOrCreateProfileAsync();
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserProfile profile, IFormFile? photo)
        {
            if (!ModelState.IsValid)
                return View(profile);

            var existing = await _repositoryWrapper.UserProfileRepository
                .FindByCondition(p => p.UserId == CurrentUserId)
                .FirstOrDefaultAsync();

            if (existing == null)
                return NotFound();

            existing.Name = profile.Name;
            existing.Email = profile.Email;
            existing.Phone = profile.Phone;

            if (photo != null && photo.Length > 0)
            {
                var ext = Path.GetExtension(photo.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("photo", "Sunt acceptate doar fișiere .jpg, .jpeg, .png, .webp");
                    return View(profile);
                }

                var uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "profiles");
                Directory.CreateDirectory(uploadsDir);

                var fileName = $"user-{CurrentUserId}{ext}";
                var filePath = Path.Combine(uploadsDir, fileName);

                DeleteFileFromWwwroot(existing.PhotoPath);

                using var stream = new FileStream(filePath, FileMode.Create);
                await photo.CopyToAsync(stream);

                existing.PhotoPath = $"/assets/images/profiles/{fileName}";
            }

            _repositoryWrapper.UserProfileRepository.Update(existing);
            _repositoryWrapper.save();

            TempData["Success"] = "Profilul a fost actualizat cu succes!";
            return RedirectToAction(nameof(Profile));
        }

        private void DeleteFileFromWwwroot(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return;
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
    }
}