using Microsoft.AspNetCore.Authorization;

namespace Rapido.Framework.Auth.Policies;

internal sealed class RoleRequirement : IAuthorizationRequirement
{
    public string Role { get; private set; }

    public RoleRequirement(string role)
    {
        Role = role;
    }
}