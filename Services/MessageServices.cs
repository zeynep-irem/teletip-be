using Google.Cloud.Firestore;
using Teletipbe.Models;

namespace Teletipbe.Services
{
    public class MessageService
    {
        private readonly FirestoreDb _firestore;

        public MessageService(FirestoreDb firestore)
        {
            _firestore = firestore;
        }

        // Mesajları getir
        public async Task<List<MessageModel>> GetMessagesAsync(string sessionId)
        {
            var messages = await _firestore.Collection("chats").Document(sessionId).Collection("messages").GetSnapshotAsync();
            return messages.Documents.Select(doc => doc.ConvertTo<MessageModel>()).ToList();
        }

        // Yeni mesaj gönder
        public async Task SendMessageAsync(string sessionId, MessageModel message)
        {
            var docRef = _firestore.Collection("chats").Document(sessionId).Collection("messages").Document();
            await docRef.SetAsync(message);
        }
    }
}
