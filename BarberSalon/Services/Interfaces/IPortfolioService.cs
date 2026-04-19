using BarberSalon.Models;

namespace BarberSalon.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<List<PortofolioItem>> GetAllPortfoliosAsync();
        Task<PortofolioItem?> GetPortfolioByIdAsync(int id);
        Task CreatePortfolioAsync(int serviceId, int workerId, string imageUrl);
        Task DeletePortfolioAsync(int id);
    }
}
