using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common;
using Rapido.Framework.Postgres;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Facades;
using Rapido.Services.Notifications.Core.Services;

namespace Rapido.Services.Notifications.Core;

public static class Extensions
{
    private const string SectionName = "emailSender";
    
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPostgres<NotificationsDbContext>(configuration);
        services.AddInitializer<NotificationsDataInitializer>();
        services
            .AddScoped<IEmailSenderFacade, EmailSenderFacade>()
            .AddScoped<ITemplateService, TemplateService>();

        var options = configuration.BindOptions<EmailSenderOptions>(SectionName);

        services
            .AddFluentEmail(options.SenderEmail, options.Sender)
            .AddSmtpSender(options.Host, options.Port);

        return services;
    }
}