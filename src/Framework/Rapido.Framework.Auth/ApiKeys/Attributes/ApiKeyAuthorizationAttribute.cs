using Microsoft.AspNetCore.Mvc;
using Rapido.Framework.Auth.ApiKeys.Filters;

namespace Rapido.Framework.Auth.ApiKeys.Attributes;

public class ApiKeyAuthorizationAttribute : ServiceFilterAttribute
{
    public ApiKeyAuthorizationAttribute() : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}