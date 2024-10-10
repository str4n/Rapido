using Microsoft.Extensions.DependencyInjection;
using Rapido.Services.Wallets.Domain.Wallets.DomainServices;

namespace Rapido.Services.Wallets.Domain;

public static class Extensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ITransferService, TransferService>();
        
        return services;
    }
}