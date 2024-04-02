using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Rapido.Framework.Contexts.Identity;

internal sealed class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
    public string Role { get; }
    public string Email { get; }
    
    public IdentityContext(ClaimsPrincipal claimsPrincipal)
    {
        IsAuthenticated = claimsPrincipal.Identity?.IsAuthenticated is true;
        UserId = IsAuthenticated ? Guid.Parse(claimsPrincipal.Identity?.Name!) : Guid.Empty;
        Role = claimsPrincipal.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        Email = claimsPrincipal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;
    }

    private IdentityContext()
    {
    }

    public static IIdentityContext Empty => new IdentityContext();
}