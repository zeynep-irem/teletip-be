using Microsoft.AspNetCore.Mvc;
using Teletipbe.Models;
using Teletipbe.Services;

namespace Teletipbe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ENabizController : ControllerBase
    {
        private readonly ENabizService _enabizService;

        public ENabizController(ENabizService enabizService)
        {
            _enabizService = enabizService;
        }

        [HttpGet("patient/{id}")]
        public async Task<IActionResult> GetPatientHealthData(string id)
        {
            try
            {
                var healthData = await _enabizService.GetPatientHealthDataAsync(id);
                return Ok(healthData);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("patient/{id}/update")]
        public async Task<IActionResult> UpdatePatientHealthData(string id, [FromBody] ENabizModel model)
        {
            await _enabizService.UpdatePatientHealthDataAsync(id, model);
            return Ok(new { message = "Health data updated successfully" });
        }
    }
}
