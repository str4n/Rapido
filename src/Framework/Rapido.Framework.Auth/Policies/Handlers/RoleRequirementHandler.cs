using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Rapido.Framework.Auth.Policies.Handlers;

internal sealed class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
{
    private static readonly string[] RoleHierarchy = ["user", "admin"];
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        var currentRole = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value.ToLowerInvariant();
        var requiredRole = requirement.Role.ToLowerInvariant();

        var indexOfCurrentRole = Array.IndexOf(RoleHierarchy, currentRole);
        var indexOfRequiredRole = Array.IndexOf(RoleHierarchy, requiredRole);

        if (indexOfCurrentRole >= indexOfRequiredRole)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        
        context.Fail();
        return Task.CompletedTask;
    }
}