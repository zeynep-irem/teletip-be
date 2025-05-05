using Microsoft.AspNetCore.Mvc;
using Teletipbe.Models;
using Teletipbe.Services;

namespace Teletipbe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentModel model)
        {
            var appointmentId = await _appointmentService.CreateAppointmentAsync(model);
            return Ok(new { message = "Appointment created", id = appointmentId });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAppointment(string id, [FromBody] AppointmentModel model)
        {
            await _appointmentService.UpdateAppointmentAsync(id, model);
            return Ok(new { message = "Appointment updated" });
        }
    }
}
