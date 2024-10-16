using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Postgres;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Facades;

namespace Rapido.Services.Notifications.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgres<NotificationsDbContext>(configuration);
        services.AddSingleton<IEmailSenderFacade, FakeEmailSenderFacade>();

        return services;
    }
}