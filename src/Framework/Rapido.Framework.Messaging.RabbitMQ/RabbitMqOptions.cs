namespace Rapido.Framework.Messaging.RabbitMQ;

public sealed class RabbitMqOptions
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Durable { get; set; }
}