namespace NotificationAPI.Service.Interfaces
{
    public interface INotificationSender
    {
        Task SendToAllAsync(string message);
    }
}
