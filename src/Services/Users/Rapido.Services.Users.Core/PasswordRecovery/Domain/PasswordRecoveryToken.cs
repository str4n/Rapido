namespace Rapido.Services.Users.Core.PasswordRecovery.Domain;

public sealed class PasswordRecoveryToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    public DateTime CreatedAt { get; set; }
}