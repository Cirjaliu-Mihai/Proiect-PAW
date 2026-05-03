namespace BarberSalon.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        IAppointmentRepository AppointmentRepository { get; }
        IReviewsRepository ReviewsRepository { get; }
        IPortofolioRepository PortofolioRepository { get; }
        IServiceRepository ServiceRepository { get; }
        IWorkerRepository WorkerRepository { get; }
        IUserProfileRepository UserProfileRepository { get; }
        IProductRepository ProductRepository { get; }
        void save();
    }
}
