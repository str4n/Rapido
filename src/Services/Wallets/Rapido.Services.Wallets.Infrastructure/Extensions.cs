using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Postgres;
using Rapido.Services.Wallets.Domain.Owners.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Infrastructure.EF;
using Rapido.Services.Wallets.Infrastructure.EF.Repositories;

namespace Rapido.Services.Wallets.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgres<WalletsDbContext>(configuration);

        services
            .AddScoped<IWalletRepository, WalletRepository>()
            .AddScoped<IIndividualOwnerRepository, IndividualOwnerRepository>()
            .AddScoped<ICorporateOwnerRepository, CorporateOwnerRepository>();
        
        return services;
    }
}