using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BarberSalon.Models;
using BarberSalon.Repositories.Interfaces;
using BarberSalon.Services.Interfaces;

namespace BarberSalon.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AppointmentsController(IAppointmentService appointmentService, IRepositoryWrapper repositoryWrapper)
        {
            _appointmentService = appointmentService;
            _repositoryWrapper = repositoryWrapper;
        }


        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }


        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name");
            ViewData["WorkerId"] = new SelectList(_repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int UserId, int ServiceId, int WorkerId, DateTime AppointmentDate, string AppointmentTime)
        {
            try
            {
                await _appointmentService.CreateAppointmentAsync(UserId, ServiceId, WorkerId, AppointmentDate, AppointmentTime);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["ServiceId"] = new SelectList(_repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name", ServiceId);
                ViewData["WorkerId"] = new SelectList(_repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name", WorkerId);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error creating appointment: " + ex.Message);
                ViewData["ServiceId"] = new SelectList(_repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name", ServiceId);
                ViewData["WorkerId"] = new SelectList(_repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name", WorkerId);
                return View();
            }
        }

        [HttpGet]
        public IActionResult GetAvailableTimes(int workerId, DateTime date, int? appointmentId = null)
        {
            var availableTimes = _appointmentService.GetAvailableTimesForWorker(workerId, date, appointmentId);
            return Json(availableTimes);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name", appointment.ServiceId);
            ViewData["WorkerId"] = new SelectList(_repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name", appointment.WorkerId);
            return View(appointment);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int UserId, int ServiceId, int WorkerId, DateTime AppointmentDate, string AppointmentTime)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            try
            {
                await _appointmentService.UpdateAppointmentAsync(id, UserId, ServiceId, WorkerId, AppointmentDate, AppointmentTime);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["ServiceId"] = new SelectList(_repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name", ServiceId);
                ViewData["WorkerId"] = new SelectList(_repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name", WorkerId);
                return View(appointment);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating appointment: " + ex.Message);
                ViewData["ServiceId"] = new SelectList(_repositoryWrapper.ServiceRepository.FindAll().ToList(), "Id", "Name", ServiceId);
                ViewData["WorkerId"] = new SelectList(_repositoryWrapper.WorkerRepository.FindAll().ToList(), "Id", "Name", WorkerId);
                return View(appointment);
            }
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id.Value);
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
            await _appointmentService.DeleteAppointmentAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Get all services
        [HttpGet]
        public IActionResult GetServices()
        {
            var services = _repositoryWrapper.ServiceRepository.FindAll()
                .Select(s => new { id = s.Id, name = s.Name }).ToList();
            return Json(services);
        }

        // GET: Get all workers
        [HttpGet]
        public IActionResult GetWorkers()
        {
            var workers = _repositoryWrapper.WorkerRepository.FindAll()
                .Select(w => new { id = w.Id, name = w.Name }).ToList();
            return Json(workers);
        }

        private bool AppointmentExists(int id)
        {
            return _appointmentService.AppointmentExists(id);
        }
    }
}
