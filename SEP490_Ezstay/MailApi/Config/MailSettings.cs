namespace MailApi.Config;

public class MailSettings
{
    public string DisplayName { get; set; } = null!;
    public string From { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool UseSSL { get; set; }
}