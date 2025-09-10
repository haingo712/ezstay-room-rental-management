namespace PaymentAPI.Config
{
    public class VnPayConfig
    {
        public string Url { get; set; }
        public string ReturnUrl { get; set; }
        public string TmnCode { get; set; }
        public string SecretKey { get; set; }
        public string Version { get; set; }
        public string Command { get; set; }
        public string OrderType { get; set; }
    }
}