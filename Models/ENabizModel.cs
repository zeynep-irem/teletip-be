using Google.Cloud.Firestore;

namespace Teletipbe.Models
{
    [FirestoreData]
    public class ENabizModel
    {
        [FirestoreProperty]
        public string PatientId { get; set; } = string.Empty;  // Hasta ID'si

        [FirestoreProperty]
        public string Diagnosis { get; set; } = string.Empty;  // Teşhis bilgisi

        [FirestoreProperty]
        public string Medications { get; set; } = string.Empty;  // İlaç bilgileri

        [FirestoreProperty]
        public string Notes { get; set; } = string.Empty;  // Ek notlar
    }
}
