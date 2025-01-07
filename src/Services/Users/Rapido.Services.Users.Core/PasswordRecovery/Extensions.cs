using Microsoft.Extensions.DependencyInjection;
using Rapido.Services.Users.Core.PasswordRecovery.Repositories;
using Rapido.Services.Users.Core.PasswordRecovery.Services;
using Rapido.Services.Users.Core.Shared.EF.Repositories;

namespace Rapido.Services.Users.Core.PasswordRecovery;

internal static class Extensions
{
    public static IServiceCollection AddPasswordRecoveryServices(this IServiceCollection services)
        => services
            .AddScoped<IRecoveryTokenGenerator, RecoveryTokenGenerator>()
            .AddScoped<IRecoveryTokenRepository, RecoveryTokenRepository>()
            .AddHostedService<RecoveryTokenCleaner>();
}