using Microsoft.AspNetCore.Mvc;
using BarberSalon.Repositories.Interfaces;
using BarberSalon.Models;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IRepositoryWrapper repositoryWrapper, IWebHostEnvironment webHostEnvironment)
        {
            _repositoryWrapper = repositoryWrapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _repositoryWrapper.ProductRepository
                .FindAll()
                .ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? photo)
        {
            if (!ModelState.IsValid)
                return View(product);

            product.PhotoPath = await SavePhotoAsync(photo) ?? "/assets/images/products/default-product.png";

            _repositoryWrapper.ProductRepository.Create(product);
            _repositoryWrapper.save();
            TempData["Success"] = "Produsul a fost adăugat cu succes!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repositoryWrapper.ProductRepository
                .FindByCondition(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? photo)
        {
            if (id != product.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(product);

            var existing = await _repositoryWrapper.ProductRepository
                .FindByCondition(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (existing == null)
                return NotFound();

            var newPath = await SavePhotoAsync(photo);
            if (newPath != null)
            {
                DeleteFileFromWwwroot(existing.PhotoPath);
                existing.PhotoPath = newPath;
            }

            existing.Name = product.Name;
            existing.Brand = product.Brand;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.StockQuantity = product.StockQuantity;

            _repositoryWrapper.ProductRepository.Update(existing);
            _repositoryWrapper.save();
            TempData["Success"] = "Produsul a fost actualizat cu succes!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repositoryWrapper.ProductRepository
                .FindByCondition(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound();

            DeleteFileFromWwwroot(product.PhotoPath);
            _repositoryWrapper.ProductRepository.Delete(product);
            _repositoryWrapper.save();
            TempData["Success"] = "Produsul a fost șters cu succes!";
            return RedirectToAction(nameof(Index));
        }

        private void DeleteFileFromWwwroot(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return;
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }

        private async Task<string?> SavePhotoAsync(IFormFile? photo)
        {
            if (photo == null || photo.Length == 0)
                return null;

            var ext = Path.GetExtension(photo.FileName).ToLowerInvariant();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowed.Contains(ext))
                return null;

            var uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "products");
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"product-{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await photo.CopyToAsync(stream);

            return $"/assets/images/products/{fileName}";
        }
    }
}
