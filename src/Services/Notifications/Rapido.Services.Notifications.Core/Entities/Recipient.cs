namespace Rapido.Services.Notifications.Core.Entities;

internal sealed class Recipient
{
    public Guid Id { get; }
    public string Email { get; private set; }

    public Recipient(Guid id, string email)
    {
        Id = id;
        Email = email;
    }

    private Recipient()
    {
    }
}