using EasyNetQ;
using Microsoft.AspNetCore.SignalR;
using SignalRHubAPI.Hub;
using SignalRHubAPI.DTO;

namespace SignalRHubAPI.Listener
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IBus _bus;
        private readonly IHubContext<NotificationHub> _hubContext;
        public RabbitMqListener(IBus bus, IHubContext<NotificationHub> hubContext)
        {
            _bus = bus;
            _hubContext = hubContext;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            _bus.PubSub.SubscribeAsync<string>("notification-events", async json =>
            {
                var message = System.Text.Json.JsonSerializer.Deserialize<NotificationDTO>(json);                
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
            });
            return Task.CompletedTask;
        }
    }
}
