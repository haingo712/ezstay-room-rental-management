    namespace SignalRHubAPI.Hub;
    using Microsoft.AspNetCore.SignalR;
    public class NotificationHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

    }