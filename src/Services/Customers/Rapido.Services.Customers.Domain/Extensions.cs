using Microsoft.Extensions.DependencyInjection;

namespace Rapido.Services.Customers.Domain;

public static class Extensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services;
    }
}