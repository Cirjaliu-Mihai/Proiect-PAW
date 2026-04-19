using BarberSalon.Models;
using BarberSalon.Repositories;
using BarberSalon.Repositories.Interfaces;
using BarberSalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public PortfolioService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<PortofolioItem>> GetAllPortfoliosAsync()
        {
            return await _repositoryWrapper.PortofolioRepository
                .FindAll()
                .Include(p => p.Service)
                .Include(p => p.Worker)
                .ToListAsync();
        }

        public async Task<PortofolioItem?> GetPortfolioByIdAsync(int id)
        {
            return await _repositoryWrapper.PortofolioRepository
                .FindByCondition(p => p.Id == id)
                .Include(p => p.Service)
                .Include(p => p.Worker)
                .FirstOrDefaultAsync();
        }

        public async Task CreatePortfolioAsync(int serviceId, int workerId, string imageUrl)
        {
            if (serviceId == 0 || workerId == 0 || string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException("ServiceId, WorkerId, and ImageUrl are required");
            }

            var portfolioItem = new PortofolioItem
            {
                ServiceId = serviceId,
                WorkerId = workerId,
                ImageUrl = imageUrl
            };

            _repositoryWrapper.PortofolioRepository.Create(portfolioItem);
            _repositoryWrapper.save();
        }

        public async Task DeletePortfolioAsync(int id)
        {
            var portfolioItem = await GetPortfolioByIdAsync(id);
            if (portfolioItem != null)
            {
                _repositoryWrapper.PortofolioRepository.Delete(portfolioItem);
                _repositoryWrapper.save();
            }
        }
    }
}

