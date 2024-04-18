using Microsoft.Extensions.DependencyInjection;

namespace Rapido.Services.Wallets.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}