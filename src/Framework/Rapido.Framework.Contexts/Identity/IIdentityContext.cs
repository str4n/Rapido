namespace Rapido.Framework.Contexts.Identity;

public interface IIdentityContext
{
    bool IsAuthenticated { get; }
    Guid UserId { get; }
    string Role { get; }
    string Email { get; }
}