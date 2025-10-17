using NotificationAPI.Enums;

namespace NotificationAPI.DTOs.Resquest
{
    public class NotifyRequest
    {
        public NotificationType NotificationType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        //public RelatedItemType RelatedItemType { get; set; }
        //public int RelatedItemId { get; set; }
    }
}
