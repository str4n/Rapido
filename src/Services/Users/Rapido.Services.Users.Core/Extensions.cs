using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Postgres;
using Rapido.Services.Users.Core.PasswordRecovery;
using Rapido.Services.Users.Core.Shared.EF;
using Rapido.Services.Users.Core.Shared.Storage;
using Rapido.Services.Users.Core.Shared.Validators;
using Rapido.Services.Users.Core.User;
using Rapido.Services.Users.Core.UserActivation;
namespace Rapido.Services.Users.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddPostgres<UsersDbContext>(configuration)
            .AddUnitOfWork<UsersDbContext>()
            .AddInitializer<UsersDataInitializer>()
            .AddScoped<ITokenStorage, HttpContextTokenStorage>()
            .AddSingleton<IPasswordValidator, PasswordValidator>()
            .AddUserServices()
            .AddUserActivationServices()
            .AddPasswordRecoveryServices();
}