using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRHubAPI.Hub;

namespace SignalRHubAPI.Controllers;

[ApiController]
[Route("api/signalR")]
public class SignalRController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRController(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        return Ok(new { Status = "Sent", Message = message });
    }
}