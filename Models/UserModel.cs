// Models/UserModel.cs
using Google.Cloud.Firestore;

namespace Teletipbe.Models
{
    [FirestoreData]
    public class UserModel
    {
        [FirestoreDocumentId]
        public string Id { get; set; } = string.Empty;

        // FirestoreProperty attribute, koleksiyondaki alan adıyla eşleşmeli
        [FirestoreProperty]
        public string Name { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Email { get; set; } = string.Empty;

        // Oluştururken sadece POST gövdesinden okunacak, Firestore’a yazılmaz
        [FirestoreProperty]
        public string Password { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Role { get; set; } = string.Empty;

        [FirestoreProperty]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
