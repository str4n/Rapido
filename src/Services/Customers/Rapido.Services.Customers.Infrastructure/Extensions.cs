using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Postgres;
using Rapido.Services.Customers.Domain.Common.Repositories;
using Rapido.Services.Customers.Domain.Corporate.Repositories;
using Rapido.Services.Customers.Domain.Individual.Repositories;
using Rapido.Services.Customers.Infrastructure.EF;
using Rapido.Services.Customers.Infrastructure.EF.Repositories;
using Rapido.Services.Customers.Infrastructure.Services;

namespace Rapido.Services.Customers.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPostgres<CustomersDbContext>(configuration)
            .AddScoped<IIndividualCustomerRepository, IndividualCustomerRepository>()
            .AddScoped<ICorporateCustomerRepository, CorporateCustomerRepository>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddHostedService<CustomerLockoutService>();;
        
        return services;
    }
}