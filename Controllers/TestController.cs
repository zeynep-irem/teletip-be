using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Google.Cloud.Firestore;
using Teletipbe.Hubs;

namespace Teletipbe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly FirestoreDb _firestore;
        private readonly IHubContext<VideoCallHub> _hubContext;

        public TestController(IHubContext<VideoCallHub> hubContext)
        {
            _firestore = FirestoreDb.Create("teletipbe");
            _hubContext = hubContext;
        }

        [HttpGet("firestore-test")]
        public async Task<IActionResult> FirestoreTest()
        {
            try
            {
                var docRef = _firestore.Collection("test_collection").Document("test_doc");
                await docRef.SetAsync(new { Name = "API Test User", Age = 30 });

                var snapshot = await docRef.GetSnapshotAsync();
                if (snapshot.Exists)
                {
                    return Ok(snapshot.ToDictionary());
                }
                return NotFound("Test document not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("start-video-call")]
        public async Task<IActionResult> StartVideoCall([FromBody] StartVideoCallRequest request)
        {
            try
            {
                // Tüm bağlı client'lara video call signal'i gönder
                await _hubContext.Clients.All.SendAsync("StartVideoCall", request.AppointmentId);

                return Ok(new
                {
                    message = "Video call signal sent successfully",
                    appointmentId = request.AppointmentId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class StartVideoCallRequest
    {
        public string AppointmentId { get; set; } = string.Empty;
    }
}