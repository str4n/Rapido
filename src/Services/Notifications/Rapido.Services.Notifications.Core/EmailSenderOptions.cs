namespace Rapido.Services.Notifications.Core;

public class EmailSenderOptions
{
    public string SenderEmail { get; set; }
    public string Sender { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}