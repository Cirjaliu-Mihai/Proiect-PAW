using BarberSalon.Models;
using BarberSalon.Repositories.Interfaces;
using BarberSalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public ReviewService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _repositoryWrapper.ReviewsRepository
                .FindAll()
                .Include(r => r.Service)
                .Include(r => r.Worker)
                .OrderByDescending(r => r.Id)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _repositoryWrapper.ReviewsRepository
                .FindByCondition(r => r.Id == id)
                .Include(r => r.Service)
                .Include(r => r.Worker)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByWorkerAsync(int workerId)
        {
            return await _repositoryWrapper.ReviewsRepository
                .FindByCondition(r => r.WorkerId == workerId)
                .Include(r => r.Service)
                .OrderByDescending(r => r.Id)
                .ToListAsync();
        }

        public async Task CreateReviewAsync(int serviceId, int workerId, int rating, string comment, string author)
        {
            // Validation
            if (serviceId == 0)
            {
                throw new ArgumentException("Please select a service");
            }

            if (workerId == 0)
            {
                throw new ArgumentException("Please select a worker");
            }

            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5");
            }

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentException("Comment is required");
            }

            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Author name is required");
            }

            var review = new Review
            {
                UserId = 1, // Default user for now
                Author = author,
                ServiceId = serviceId,
                WorkerId = workerId,
                Rating = rating,
                Comment = comment
            };

            _repositoryWrapper.ReviewsRepository.Create(review);
            _repositoryWrapper.save();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await GetReviewByIdAsync(id);
            if (review != null)
            {
                _repositoryWrapper.ReviewsRepository.Delete(review);
                _repositoryWrapper.save();
            }
        }
    }
}
