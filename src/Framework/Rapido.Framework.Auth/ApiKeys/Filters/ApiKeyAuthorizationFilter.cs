using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rapido.Framework.Auth.ApiKeys.Vault;

namespace Rapido.Framework.Auth.ApiKeys.Filters;

internal sealed class ApiKeyAuthorizationFilter : IAuthorizationFilter
{
    private readonly IApiKeyVault _vault;

    public ApiKeyAuthorizationFilter(IApiKeyVault vault)
    {
        _vault = vault;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string apiKey = context.HttpContext.Request.Headers[ApiKey.HeaderName];

        if (!_vault.Validate(apiKey).Succeeded)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}