using Google.Cloud.Firestore;
using Teletipbe.Models;

namespace Teletipbe.Services
{
    public class ENabizService
    {
        private readonly FirestoreDb _firestore;

        public ENabizService(FirestoreDb firestore)
        {
            _firestore = firestore;
        }

        // Hasta sağlık verilerini getir
        public async Task<ENabizModel> GetPatientHealthDataAsync(string patientId)
        {
            var doc = await _firestore.Collection("health_data").Document(patientId).GetSnapshotAsync();
            if (doc.Exists)
            {
                return doc.ConvertTo<ENabizModel>();
            }
            throw new Exception("Health data not found for the patient.");
        }

        // Sağlık verilerini güncelle
        public async Task UpdatePatientHealthDataAsync(string patientId, ENabizModel healthData)
        {
            var docRef = _firestore.Collection("health_data").Document(patientId);
            await docRef.SetAsync(healthData, SetOptions.Overwrite);
        }
    }
}
