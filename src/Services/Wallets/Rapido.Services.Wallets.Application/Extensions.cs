using Microsoft.Extensions.DependencyInjection;
using Rapido.Services.Wallets.Application.Wallets.Clients;

namespace Rapido.Services.Wallets.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICurrencyApiClient, CurrencyApiClient>();
        
        return services;
    }
}