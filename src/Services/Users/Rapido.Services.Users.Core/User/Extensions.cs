using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Services.Users.Core.Shared.EF.Repositories;
using Rapido.Services.Users.Core.User.Repositories;
using Rapido.Services.Users.Core.User.Services;

namespace Rapido.Services.Users.Core.User;

internal static class Extensions
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
        => services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IPasswordManager, PasswordManager>()
            .AddSingleton<IPasswordHasher<User.Domain.User>, PasswordHasher<User.Domain.User>>();
}