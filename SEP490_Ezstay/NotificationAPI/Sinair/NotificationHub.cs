using Microsoft.AspNetCore.SignalR;

namespace NotificationAPI.Sinair
{
    public class NotificationHub :Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Có thể dùng Context.UserIdentifier để lấy UserId từ token (nếu cần)
            await base.OnConnectedAsync();
        }
    }
}
