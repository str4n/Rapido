using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Postgres;
using Rapido.Services.Users.Core.EF;

namespace Rapido.Services.Users.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddPostgres<UsersDbContext>(configuration)
            .AddInitializer<UsersDataInitializer>();
}