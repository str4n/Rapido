using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common;
using Rapido.Framework.Redis.Cache;

namespace Rapido.Framework.Redis;

public static class Extensions
{
    private const string SectionName = "redis";

    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.BindOptions<RedisOptions>(SectionName);

        services.AddStackExchangeRedisCache(r =>
        {
            r.Configuration = options.ConnectionString;
        });

        services.AddScoped<ICache, RedisCache>();
        
        return services;
    }
}