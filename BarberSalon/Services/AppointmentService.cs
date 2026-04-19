using BarberSalon.Models;
using BarberSalon.Models.Enums;
using BarberSalon.Repositories;
using BarberSalon.Repositories.Interfaces;
using BarberSalon.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AppointmentService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _repositoryWrapper.AppointmentRepository
                .FindAll()
                .Include(a => a.Service)
                .Include(a => a.Worker)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.AppointmentTime)
                .ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            return await _repositoryWrapper.AppointmentRepository
                .FindByCondition(a => a.Id == id)
                .Include(a => a.Service)
                .Include(a => a.Worker)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAppointmentAsync(int userId, int serviceId, int workerId, DateTime appointmentDate, string appointmentTime)
        {
            if (serviceId == 0 || workerId == 0 || string.IsNullOrEmpty(appointmentTime))
            {
                throw new ArgumentException("Please fill in all required fields");
            }

            if (!TimeOnly.TryParse(appointmentTime, out TimeOnly time))
            {
                throw new ArgumentException("Invalid time format");
            }

            var appointment = new Appointment
            {
                UserId = userId,
                ServiceId = serviceId,
                WorkerId = workerId,
                AppointmentDate = appointmentDate,
                AppointmentTime = time,
                Status = Status.Confirmed,
                CreatedAt = DateTime.Now
            };

            _repositoryWrapper.AppointmentRepository.Create(appointment);
            _repositoryWrapper.save();
        }

        public async Task UpdateAppointmentAsync(int id, int userId, int serviceId, int workerId, DateTime appointmentDate, string appointmentTime)
        {
            var appointment = await GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                throw new ArgumentException("Appointment not found");
            }

            if (serviceId == 0 || workerId == 0 || string.IsNullOrEmpty(appointmentTime))
            {
                throw new ArgumentException("Please fill in all required fields");
            }

            if (!TimeOnly.TryParse(appointmentTime, out TimeOnly time))
            {
                throw new ArgumentException("Invalid time format");
            }

            appointment.UserId = userId;
            appointment.ServiceId = serviceId;
            appointment.WorkerId = workerId;
            appointment.AppointmentDate = appointmentDate;
            appointment.AppointmentTime = time;
            appointment.UpdatedAt = DateTime.Now;

            _repositoryWrapper.AppointmentRepository.Update(appointment);
            _repositoryWrapper.save();
        }

        public async Task DeleteAppointmentAsync(int id)
        {
            var appointment = await GetAppointmentByIdAsync(id);
            if (appointment != null)
            {
                _repositoryWrapper.AppointmentRepository.Delete(appointment);
                _repositoryWrapper.save();
            }
        }

        public List<string> GetAvailableTimesForWorker(int workerId, DateTime date, int? appointmentId = null)
        {
            var appointments = _repositoryWrapper.AppointmentRepository
                .FindByCondition(a => a.WorkerId == workerId && a.AppointmentDate.Date == date.Date)
                .Select(a => new { a.Id, a.AppointmentTime })
                .ToList();

            if (appointmentId.HasValue)
            {
                appointments = appointments.Where(a => a.Id != appointmentId).ToList();
            }

            var bookedTimes = appointments.Select(a => a.AppointmentTime).ToList();

            var availableTimes = new List<string>();
            var startTime = new TimeOnly(9, 0);
            var endTime = new TimeOnly(17, 0);
            var interval = 60;

            var currentTime = startTime;
            while (currentTime < endTime)
            {
                if (!bookedTimes.Contains(currentTime))
                {
                    availableTimes.Add(currentTime.ToString("HH:mm"));
                }
                currentTime = currentTime.AddMinutes(interval);
            }

            return availableTimes;
        }

        public async Task<List<dynamic>> GetAllServicesAsync()
        {
            return await _repositoryWrapper.ServiceRepository
                .FindAll()
                .Select(s => new { id = s.Id, name = s.Name })
                .Cast<dynamic>()
                .ToListAsync();
        }

        public async Task<List<dynamic>> GetAllWorkersAsync()
        {
            return await _repositoryWrapper.WorkerRepository
                .FindAll()
                .Select(w => new { id = w.Id, name = w.Name })
                .Cast<dynamic>()
                .ToListAsync();
        }

        public bool AppointmentExists(int id)
        {
            return _repositoryWrapper.AppointmentRepository
                .FindByCondition(a => a.Id == id)
                .Any();
        }
    }
}
