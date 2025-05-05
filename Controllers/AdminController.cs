using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Teletipbe.Models;    // UserModel’ın namespace’i
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Teletipbe.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly FirestoreDb _db;
        public AdminController(FirestoreDb db) => _db = db;

        // 1) Tüm kullanıcıları listele
        [HttpGet("users")]
        public async Task<IActionResult> ListUsers()
        {
            var snap = await _db.Collection("users").GetSnapshotAsync();
            var list = snap.Documents.Select(d =>
            {
                var model = d.ConvertTo<UserModel>();
                model.Id = d.Id;                  // Document ID'yi de ata
                return model;
            });
            return Ok(list);
        }

        // 2) Yeni kullanıcı ekle
        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            // Eğer client tarafı id gönderiyorsa, o dokümana set et; yoksa otomatik yeni id alın
            DocumentReference docRef = string.IsNullOrEmpty(model.Id)
                ? _db.Collection("users").Document()
                : _db.Collection("users").Document(model.Id);

            // Id alanını Firestore’a göndermiyoruz, sadece veri özelliklerini
            var data = new Dictionary<string, object>
            {
                ["Name"] = model.Name,
                ["Email"] = model.Email,
                ["Role"] = model.Role,
                ["PhoneNumber"] = model.PhoneNumber
            };

            await docRef.SetAsync(data);
            return CreatedAtAction(nameof(GetUser), new { id = docRef.Id }, new { id = docRef.Id });
        }

        // 3) Tek bir kullanıcıyı getir
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var snap = await _db.Collection("users").Document(id).GetSnapshotAsync();
            if (!snap.Exists) return NotFound();

            var model = snap.ConvertTo<UserModel>();
            model.Id = snap.Id;
            return Ok(model);
        }

        // 4) Kullanıcı güncelle
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserModel model)
        {
            var updates = new Dictionary<string, object>();
            if (model.Name != null) updates["Name"] = model.Name;
            if (model.Email != null) updates["Email"] = model.Email;
            if (model.Role != null) updates["Role"] = model.Role;
            if (model.PhoneNumber != null) updates["PhoneNumber"] = model.PhoneNumber;

            if (updates.Count == 0) return BadRequest("Güncellenecek alan yok.");

            await _db.Collection("users").Document(id).UpdateAsync(updates);
            return NoContent();
        }

        // 5) Kullanıcı sil
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _db.Collection("users").Document(id).DeleteAsync();
            return NoContent();
        }
    }

}
