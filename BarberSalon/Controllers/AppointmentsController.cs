using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarberSalon.Data;
using BarberSalon.Models;
using BarberSalon.Models.Enums;

namespace BarberSalon.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly BarberSalonContext _context;

        public AppointmentsController(BarberSalonContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var barberSalonContext = _context.Appointment.Include(a => a.Service).Include(a => a.Worker);
            return View(await barberSalonContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Service)
                .Include(a => a.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name");
            ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name");
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int UserId, int ServiceId, int WorkerId, DateTime AppointmentDate, string AppointmentTime)
        {
            if (ServiceId == 0 || WorkerId == 0 || string.IsNullOrEmpty(AppointmentTime))
            {
                ModelState.AddModelError("", "Please fill in all required fields");
                ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", ServiceId);
                ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", WorkerId);
                return View();
            }

            try
            {
                var service = await _context.Service.FindAsync(ServiceId);
                var worker = await _context.Worker.FindAsync(WorkerId);

                if (service == null || worker == null)
                {
                    ModelState.AddModelError("", "Invalid service or worker selected");
                    ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", ServiceId);
                    ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", WorkerId);
                    return View();
                }

                if (!TimeOnly.TryParse(AppointmentTime, out TimeOnly appointmentTime))
                {
                    ModelState.AddModelError("AppointmentTime", "Invalid time format");
                    ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", ServiceId);
                    ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", WorkerId);
                    return View();
                }

                var appointment = new Appointment
                {
                    UserId = UserId,
                    ServiceId = ServiceId,
                    Service = service,
                    WorkerId = WorkerId,
                    Worker = worker,
                    AppointmentDate = AppointmentDate,
                    AppointmentTime = appointmentTime,
                    Status = Status.Confirmed,
                    CreatedAt = DateTime.Now
                };

                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating appointment: " + ex.Message);
                ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", ServiceId);
                ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", WorkerId);
                return View();
            }
        }

        // GET: Get available times for a worker on a specific date
        [HttpGet]
        public IActionResult GetAvailableTimes(int workerId, DateTime date, int? appointmentId = null)
        {
            var appointments = _context.Appointment
                .Where(a => a.WorkerId == workerId && a.AppointmentDate.Date == date.Date)
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

            return Json(availableTimes);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", appointment.ServiceId);
            ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", appointment.WorkerId);
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int UserId, int ServiceId, int WorkerId, DateTime AppointmentDate, string AppointmentTime)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            if (id != appointment.Id)
            {
                return NotFound();
            }

            try
            {
                if (ServiceId == 0 || WorkerId == 0 || string.IsNullOrEmpty(AppointmentTime))
                {
                    ModelState.AddModelError("", "Please fill in all required fields");
                    ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", ServiceId);
                    ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", WorkerId);
                    return View(appointment);
                }

                var service = await _context.Service.FindAsync(ServiceId);
                var worker = await _context.Worker.FindAsync(WorkerId);

                if (service == null || worker == null)
                {
                    ModelState.AddModelError("", "Invalid service or worker selected");
                    ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", ServiceId);
                    ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", WorkerId);
                    return View(appointment);
                }

                if (!TimeOnly.TryParse(AppointmentTime, out TimeOnly appointmentTime))
                {
                    ModelState.AddModelError("AppointmentTime", "Invalid time format");
                    ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", ServiceId);
                    ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", WorkerId);
                    return View(appointment);
                }

                appointment.UserId = UserId;
                appointment.ServiceId = ServiceId;
                appointment.Service = service;
                appointment.WorkerId = WorkerId;
                appointment.Worker = worker;
                appointment.AppointmentDate = AppointmentDate;
                appointment.AppointmentTime = appointmentTime;
                appointment.UpdatedAt = DateTime.Now;

                _context.Update(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(appointment.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating appointment: " + ex.Message);
                ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", ServiceId);
                ViewData["WorkerId"] = new SelectList(_context.Worker, "Id", "Name", WorkerId);
                return View(appointment);
            }
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .Include(a => a.Service)
                .Include(a => a.Worker)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Get all services
        [HttpGet]
        public IActionResult GetServices()
        {
            var services = _context.Service.Select(s => new { id = s.Id, name = s.Name }).ToList();
            return Json(services);
        }

        // GET: Get all workers
        [HttpGet]
        public IActionResult GetWorkers()
        {
            var workers = _context.Worker.Select(w => new { id = w.Id, name = w.Name }).ToList();
            return Json(workers);
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.Id == id);
        }
    }
}
