using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common;
using Rapido.Framework.Redis;
using Rapido.Services.Currencies.Core.Clients;
using Rapido.Services.Currencies.Core.Services;

namespace Rapido.Services.Currencies.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCaching(configuration);

        services.AddTransient<IExchangeRateApiClient, ExchangeRateApiClient>();
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
        services.AddHostedService<ExchangeRateLoader>();
        services.AddSingleton<ExchangeRateLoader>();
        
        return services;
    }
}