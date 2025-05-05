using Microsoft.AspNetCore.Mvc;
using Google.Cloud.Firestore;

namespace Teletipbe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly FirestoreDb _firestore;

        public TestController()
        {
            _firestore = FirestoreDb.Create("teletipbe");
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
    }
}
