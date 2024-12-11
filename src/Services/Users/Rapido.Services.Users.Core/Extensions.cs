using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Postgres;
using Rapido.Services.Users.Core.EF;
using Rapido.Services.Users.Core.EF.Repositories;
using Rapido.Services.Users.Core.Entities.User;
using Rapido.Services.Users.Core.Repositories;
using Rapido.Services.Users.Core.Services;
using Rapido.Services.Users.Core.Storage;
using Rapido.Services.Users.Core.Validators;

namespace Rapido.Services.Users.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddPostgres<UsersDbContext>(configuration)
            .AddInitializer<UsersDataInitializer>()
            .AddScoped<ITokenStorage, HttpContextTokenStorage>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IActivationTokenRepository, ActivationTokenRepository>()
            .AddScoped<IActivationTokenGenerator, ActivationTokenGenerator>()
            .AddScoped<ISignUpValidator, SignUpValidator>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
}