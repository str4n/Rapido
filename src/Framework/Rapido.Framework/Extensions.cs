using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Api.Exceptions;
using Rapido.Framework.Api.Swagger;
using Rapido.Framework.Auth;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Framework.Messaging.RabbitMQ;
using Rapido.Framework.Observability.Logging;

namespace Rapido.Framework;

public static class Extensions
{
    private const string SectionName = "app";
    
    public static WebApplicationBuilder AddFramework(this WebApplicationBuilder builder)
    {
        var appOptions = builder.Configuration.GetSection(SectionName).BindOptions<AppOptions>();
        var appInfo = new AppInfo(appOptions.Name, appOptions.Version);

        builder.Host
            .UseLogging(builder.Configuration);
        
        builder.Services.AddSingleton(appInfo);
        
        builder.Services
            .AddExceptionHandling()
            .AddBaseFeatures(builder.Configuration)
            .AddHttpContextAccessor()
            .AddContexts()
            .AddMemoryCache()
            .AddEndpointsApiExplorer()
            .AddHttpClient()
            .AddSwaggerDocs(builder.Configuration)
            .AddAuth(builder.Configuration);

        builder.Services
            .AddCommands()
            .AddQueries()
            .AddDomainEvents()
            .AddDispatcher();

        builder.Services
            .AddLogging(builder.Configuration);

        builder.Services
            .AddRabbitMq(builder.Configuration);

        return builder;
    }

    public static WebApplication UseFramework(this WebApplication app)
    {
        app
            .UseExceptionHandling()
            .UseSwaggerDocs()
            .UseAuthentication()
            .UseRouting()
            .UseAuthorization();

        return app;
    }
}