using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Auth.ApiKeys.Filters;
using Rapido.Framework.Auth.ApiKeys.Vault;
using Rapido.Framework.Common;

namespace Rapido.Framework.Auth.ApiKeys;

internal static class Extensions
{
    private const string SectionName = "apiKeys";
    
    public static IServiceCollection AddApiKeyAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);
        var options = section.BindOptions<ApiKeyOptions>();
      
        services.Configure<ApiKeyOptions>(section);
        services.AddSingleton(options);

        services.AddSingleton<IApiKeyVault, UserSecretsApiKeyVault>();
        services.AddSingleton<ApiKeyAuthorizationFilter>();

        return services;
    }
}