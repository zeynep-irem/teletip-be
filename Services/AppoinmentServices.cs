using Google.Cloud.Firestore;
using Teletipbe.Models;

namespace Teletipbe.Services
{
    public class AppointmentService
    {
        private readonly FirestoreDb _firestore;

        public AppointmentService(FirestoreDb firestore)
        {
            _firestore = firestore;
        }

        // Tüm randevuları getir
        public async Task<List<AppointmentModel>> GetAllAppointmentsAsync()
        {
            var appointments = await _firestore.Collection("appointments").GetSnapshotAsync();
            return appointments.Documents.Select(doc => doc.ConvertTo<AppointmentModel>()).ToList();
        }

        // Yeni randevu oluştur
        public async Task<string> CreateAppointmentAsync(AppointmentModel appointment)
        {
            try
            {
                // Yeni bir belge referansı oluştur
                var docRef = _firestore.Collection("appointments").Document();

                // Firestore'un otomatik oluşturduğu id'yi modelin id özelliğine ata
                appointment.Id = docRef.Id;

                // Belgeyi Firestore'a ekle
                await docRef.SetAsync(appointment);

                // Oluşturulan belge id'sini döndür
                return docRef.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating appointment: {ex.Message}");
            }
        }

        // Randevuyu güncelle
        public async Task UpdateAppointmentAsync(string id, AppointmentModel appointment)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("UpdateAppointmentAsync içinde: id boş olamaz.");
            }

            var docRef = _firestore.Collection("appointments").Document(id);
            await docRef.SetAsync(appointment, SetOptions.Overwrite);
        }

    }
}


