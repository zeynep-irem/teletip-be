using Microsoft.AspNetCore.SignalR;

namespace Teletipbe.Hubs
{
    public class VideoCallHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var http = Context.GetHttpContext();
            if (http.Request.Query.TryGetValue("appointmentId", out var ids))
            {
                var appointmentId = ids.First();
                await Groups.AddToGroupAsync(Context.ConnectionId, appointmentId);
            }
            await base.OnConnectedAsync();
        }

        // Burada kesinlikle "signalData" olarak gönderiyoruz
        public async Task SignalData(object data)
        {
            var appointmentId = Context.GetHttpContext()!
                                       .Request.Query["appointmentId"];
            await Clients.OthersInGroup(appointmentId)
                         .SendAsync("signalData", data);
        }

        public async Task SendMessage(string appointmentId, string user, string text)
            => await Clients.Group(appointmentId)
                            .SendAsync("ReceiveMessage", user, text);

        public async Task StartVideoCall(string appointmentId)
            => await Clients.Group(appointmentId)
                            .SendAsync("StartVideoCall", appointmentId);
    }
}
