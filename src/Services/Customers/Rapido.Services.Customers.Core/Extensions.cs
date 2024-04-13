using MassTransit.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Postgres;
using Rapido.Services.Customers.Core.Clients;
using Rapido.Services.Customers.Core.EF;
using Rapido.Services.Customers.Core.EF.Repositories;
using Rapido.Services.Customers.Core.Events;
using Rapido.Services.Customers.Core.Repositories;
using Rapido.Services.Customers.Core.Services;

namespace Rapido.Services.Customers.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddPostgres<CustomersDbContext>(configuration)
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddSingleton<IUserApiClient, UserApiClient>()
            .AddHostedService<CustomerLockoutService>();
}