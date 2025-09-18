namespace AuthApi.DTO.Request
{
    public class UpdateEmailRequest
    {
        public string OldEmail { get; set; }
        public string NewEmail { get; set; }
    }
}
