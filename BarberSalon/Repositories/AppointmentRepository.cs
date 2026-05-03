using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Models.Enums;
using BarberSalon.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BarberSalon.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(BarberSalonContext barberContext) : base(barberContext)
        {
        }

        public async Task FinalizePassedAppointmentsAsync()
        {
            var now = DateTime.Now;
            var currentTime = TimeOnly.FromDateTime(now);

            var passed = await BarberContext.Set<Appointment>()
                .Where(a =>
                    a.Status == Status.Confirmed &&
                    (a.AppointmentDate < now.Date ||
                     (a.AppointmentDate == now.Date && a.AppointmentTime < currentTime)))
                .ToListAsync();

            foreach (var appointment in passed)
            {
                appointment.Status = Status.Finalized;
                appointment.UpdatedAt = DateTime.Now;
            }

            await BarberContext.SaveChangesAsync();
        }
    }
}