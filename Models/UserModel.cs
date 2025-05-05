using Google.Cloud.Firestore;

namespace Teletipbe.Models
{
    [FirestoreData]

    public class UserModel
    {
        [FirestoreProperty]
        public string Id { get; set; } = string.Empty;  // Kullanıcı ID'si (Firestore Document ID)

        [FirestoreProperty]
        public string Name { get; set; } = string.Empty;  // Kullanıcı adı

        [FirestoreProperty]
        public string Email { get; set; } = string.Empty;  // Kullanıcı e-postası

        [FirestoreProperty]
        public string Role { get; set; } = "Patient";  // Rol (Patient veya Doctor)

        [FirestoreProperty]
        public string PhoneNumber { get; set; } = string.Empty;  // Telefon numarası
    }
}
