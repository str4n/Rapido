using Microsoft.Extensions.DependencyInjection;
using Rapido.Services.Customers.Application.Common.Clients;

namespace Rapido.Services.Customers.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
        => services
            .AddSingleton<IUserApiClient, UserApiClient>();
}