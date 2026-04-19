using BarberSalon.Models;
using BarberSalon.Models.Enums;
using BarberSalon.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(int id);
        Task CreateAppointmentAsync(int userId, int serviceId, int workerId, DateTime appointmentDate, string appointmentTime);
        Task UpdateAppointmentAsync(int id, int userId, int serviceId, int workerId, DateTime appointmentDate, string appointmentTime);
        Task DeleteAppointmentAsync(int id);
        List<string> GetAvailableTimesForWorker(int workerId, DateTime date, int? appointmentId = null);
        Task<List<dynamic>> GetAllServicesAsync();
        Task<List<dynamic>> GetAllWorkersAsync();
        bool AppointmentExists(int id);
    }
}
