using Google.Cloud.Firestore;

namespace Teletipbe.Models
{
    [FirestoreData]
    public class AppointmentModel
    {
        [FirestoreProperty("id")]
        public string Id { get; set; }

        [FirestoreProperty("patientId")]
        public string PatientId { get; set; }

        [FirestoreProperty("doctorId")]
        public string DoctorId { get; set; }

        [FirestoreProperty("date")]
        public string Date { get; set; }

        [FirestoreProperty("time")]
        public string Time { get; set; }

        [FirestoreProperty("status")]
        public string Status { get; set; } = "Pending";

    }
}
