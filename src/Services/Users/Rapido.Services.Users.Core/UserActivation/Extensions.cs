using Microsoft.Extensions.DependencyInjection;
using Rapido.Services.Users.Core.Shared.EF.Repositories;
using Rapido.Services.Users.Core.UserActivation.Repositories;
using Rapido.Services.Users.Core.UserActivation.Services;

namespace Rapido.Services.Users.Core.UserActivation;

internal static class Extensions
{
    public static IServiceCollection AddUserActivationServices(this IServiceCollection services)
        => services
            .AddScoped<IActivationTokenRepository, ActivationTokenRepository>()
            .AddScoped<IActivationTokenGenerator, ActivationTokenGenerator>()
            .AddHostedService<ActivationTokenCleaner>();
}