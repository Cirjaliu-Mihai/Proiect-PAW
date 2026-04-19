using BarberSalon.Models;

namespace BarberSalon.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByWorkerAsync(int workerId);
        Task CreateReviewAsync(int serviceId, int workerId, int rating, string comment, string author);
        Task DeleteReviewAsync(int id);
    }
}
