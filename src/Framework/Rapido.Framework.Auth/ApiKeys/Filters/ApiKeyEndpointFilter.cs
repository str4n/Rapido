using Microsoft.AspNetCore.Http;
using Rapido.Framework.Auth.ApiKeys.Vault;

namespace Rapido.Framework.Auth.ApiKeys.Filters;

public sealed class ApiKeyEndpointFilter : IEndpointFilter
{
    private const string ApiKeyHeaderName = "X-API-Key";
    private readonly IApiKeyVault _vault;

    public ApiKeyEndpointFilter(IApiKeyVault vault)
    {
        _vault = vault;
    }
    
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        string apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];

        if (apiKey is null)
        {
            return Results.Unauthorized();
        }
        
        if (!_vault.Validate(apiKey).Succeeded)
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }
}