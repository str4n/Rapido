using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rapido.Framework.Auth.ApiKeys.Vault;

namespace Rapido.Framework.Auth.ApiKeys.Filters;

internal sealed class ApiKeyAuthorizationFilter : IAuthorizationFilter
{
    private readonly IApiKeyVault _vault;
    private const string ApiKeyHeaderName = "X-API-Key";

    public ApiKeyAuthorizationFilter(IApiKeyVault vault)
    {
        _vault = vault;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];

        if (!_vault.Validate(apiKey).Succeeded)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}