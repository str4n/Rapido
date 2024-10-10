using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Postgres;
using Rapido.Services.Customers.Core.Common.Clients;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.EF;
using Rapido.Services.Customers.Core.Common.EF.Repositories;
using Rapido.Services.Customers.Core.Common.Services;

namespace Rapido.Services.Customers.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IUserApiClient, UserApiClient>();
        
        services
            .AddPostgres<CustomersDbContext>(configuration)
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddHostedService<CustomerLockoutService>();

        return services;
    }
}