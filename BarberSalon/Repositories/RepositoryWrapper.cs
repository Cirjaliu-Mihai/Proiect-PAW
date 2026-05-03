using BarberSalon.Data;
using BarberSalon.Repositories.Interfaces;

namespace BarberSalon.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private BarberSalonContext _barberContext;

        private IAppointmentRepository? _appointmentRepository;
        private IReviewsRepository? _reviewsRepository;
        private IPortofolioRepository? _portofolioRepository;
        private IServiceRepository? _serviceRepository;
        private IWorkerRepository? _workerRepository;
        private IUserProfileRepository? _userProfileRepository;
        private IProductRepository? _productRepository;

        public IAppointmentRepository AppointmentRepository
        {
            get
            {
                if (_appointmentRepository == null)
                {
                    _appointmentRepository = new AppointmentRepository(_barberContext);
                }
                return _appointmentRepository;
            }
        }

        public IReviewsRepository ReviewsRepository
        {
            get
            {
                if (_reviewsRepository == null)
                {
                    _reviewsRepository = new ReviewRepository(_barberContext);
                }
                return _reviewsRepository;
            }
        }

        public IPortofolioRepository PortofolioRepository
        {
            get
            {
                if (_portofolioRepository == null)
                {
                    _portofolioRepository = new PortofolioRepository(_barberContext);
                }
                return _portofolioRepository;
            }
        }

        public IServiceRepository ServiceRepository
        {
            get
            {
                if (_serviceRepository == null)
                {
                    _serviceRepository = new ServiceRepository(_barberContext);
                }
                return _serviceRepository;
            }
        }

        public IWorkerRepository WorkerRepository
        {
            get
            {
                if (_workerRepository == null)
                {
                    _workerRepository = new WorkerRepository(_barberContext);
                }
                return _workerRepository;
            }
        }

        public RepositoryWrapper(BarberSalonContext barberContext)
        {
            _barberContext = barberContext;
        }

        public IUserProfileRepository UserProfileRepository
        {
            get
            {
                if (_userProfileRepository == null)
                {
                    _userProfileRepository = new UserProfileRepository(_barberContext);
                }
                return _userProfileRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_barberContext);
                }
                return _productRepository;
            }
        }

        public void save()
        {
            _barberContext.SaveChanges();
        }
    }
}
