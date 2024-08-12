using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Postgres;
using Rapido.Services.Urls.Core.EF;
using Rapido.Services.Urls.Core.Services;

namespace Rapido.Services.Urls.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgres<UrlsDbContext>(configuration);

        services.AddSingleton<IUrlAliasGenerator, UrlAliasGenerator>();
        services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        
        return services;
    }
    
}