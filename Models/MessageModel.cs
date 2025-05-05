using Google.Cloud.Firestore;

namespace Teletipbe.Models
{
    [FirestoreData] // Firestore ile model eşlemesi için gerekli
    public class MessageModel
    {
        [FirestoreProperty]
        public string Sender { get; set; } = string.Empty; // Mesaj gönderen kişi

        [FirestoreProperty]
        public string Content { get; set; } = string.Empty; // Mesaj içeriği

        [FirestoreProperty]
        public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); // Zaman damgası
    }
}
