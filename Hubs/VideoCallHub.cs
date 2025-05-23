using Microsoft.AspNetCore.SignalR;

namespace Teletipbe.Hubs
{
    public class VideoCallHub : Hub
    {
        
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"🔌 Client connected: {Context.ConnectionId}");

            var http = Context.GetHttpContext();
            if (http.Request.Query.TryGetValue("appointmentId", out var ids))
            {
                var appointmentId = ids.First();
                await Groups.AddToGroupAsync(Context.ConnectionId, appointmentId);
                Console.WriteLine($"👥 Added to group: {appointmentId}");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine($"❌ Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        // Frontend'den çağrılan JoinUserGroup metodu
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            Console.WriteLine($"👤 User {userId} joined group with connection {Context.ConnectionId}");
        }

        // Frontend'den çağrılan SendSignal metodu (WebRTC için)
        public async Task SendSignal(string appointmentId, object signalData)
        {
            Console.WriteLine($"📤 Sending signal data for appointment: {appointmentId}");
            await Clients.OthersInGroup($"appointment_{appointmentId}")
                         .SendAsync("SignalData", signalData);
        }

        // Mevcut SignalData metodu (eski uyumluluk için)
        public async Task SignalData(object data)
        {
            var appointmentId = Context.GetHttpContext()!
                                       .Request.Query["appointmentId"];
            Console.WriteLine($"📤 Legacy signal data for appointment: {appointmentId}");
            await Clients.OthersInGroup(appointmentId)
                         .SendAsync("SignalData", data);
        }

        // Randevu grubuna katılma
        public async Task JoinAppointmentGroup(string appointmentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"appointment_{appointmentId}");
            Console.WriteLine($"📅 Joined appointment group: {appointmentId}");
        }

        // Mesaj gönderme
        public async Task SendMessage(string appointmentId, string user, string text)
        {
            Console.WriteLine($"💬 Message from {user} in appointment {appointmentId}: {text}");
            await Clients.Group($"appointment_{appointmentId}")
                         .SendAsync("ReceiveMessage", user, text);
        }

        // Video call başlatma (tüm kullanıcılara broadcast)
        public async Task StartVideoCall(string appointmentId)
        {
            Console.WriteLine($"🎥 Starting video call for appointment: {appointmentId}");

            // Appointment grubundakilere gönder
            await Clients.Group($"appointment_{appointmentId}")
                         .SendAsync("StartVideoCall", appointmentId);

            // Tüm bağlı kullanıcılara da gönder (test için)
            await Clients.All.SendAsync("StartVideoCall", appointmentId);
        }

        // Test için video call başlatma
        public async Task StartTestVideoCall(string testAppointmentId)
        {
            Console.WriteLine($"🧪 Starting TEST video call: {testAppointmentId}");
            await Clients.All.SendAsync("StartVideoCall", testAppointmentId);
        }
    }
}

