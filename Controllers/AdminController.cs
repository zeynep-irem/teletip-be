// Controllers/AdminController.cs
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Teletipbe.Models;    // UserModel zaten doğru
using Teletipbe.Services;

namespace Teletipbe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly FirestoreDb _db;
        private readonly FirebaseService _fb;

        public AdminController(FirestoreDb db, FirebaseService fb)
        {
            _db = db;
            _fb = fb;
        }

        // 1) Tüm kullanıcıları listele (Firestore + Auth bilgilerini birleştiriyoruz)
        [HttpGet("users")]
        public async Task<IActionResult> ListUsers()
        {
            var snap = await _db.Collection("users").GetSnapshotAsync();
            var list = new List<UserModel>();

            foreach (var doc in snap.Documents)
            {
                var model = doc.ConvertTo<UserModel>();
                model.Id = doc.Id;

                try
                {
                    var au = await _fb.GetUserByUidAsync(doc.Id);
                    model.Email = au.Email ?? model.Email;
                }
                catch
                {
                    // Auth’da olmayansa atla
                }

                list.Add(model);
            }

            return Ok(list);
        }

        // 2) Yeni kullanıcı ekle
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            // 2.1) Auth’da yarat
            var newUid = await _fb.CreateUserAsync(
                email: model.Email,
                password: model.Password
            );

            // 2.2) Firestore’a sadece metadata yaz
            var docRef = _db.Collection("users").Document(newUid);
            await docRef.SetAsync(new Dictionary<string, object>
            {
                ["email"] = model.Email,
                ["name"] = model.Name,
                ["password"] = model.Password,
                ["role"] = model.Role,
                ["phoneNumber"] = model.PhoneNumber,

            });

            return CreatedAtAction(nameof(ListUsers), new { id = newUid }, new { id = newUid });
        }

        // 3) Kullanıcı metadata güncelle (rol / tel)
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserModel model)
        {
            var updates = new Dictionary<string, object>();
            if (model.Role != null) updates["role"] = model.Role;
            if (model.PhoneNumber != null) updates["phoneNumber"] = model.PhoneNumber;
            if (!updates.Any()) return BadRequest("Güncellenecek alan yok.");

            await _db.Collection("users").Document(id).UpdateAsync(updates);
            return NoContent();
        }

        // 4) Sil (önce Auth, sonra Firestore)
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _fb.DeleteUserAsync(id);
            await _db.Collection("users").Document(id).DeleteAsync();
            return NoContent();
        }
    }
}
