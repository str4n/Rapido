﻿using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Api.CORS;
using Rapido.Framework.Api.Exceptions;
using Rapido.Framework.Api.Swagger;
using Rapido.Framework.Auth;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Dispatchers;
using Rapido.Framework.Contexts;
using Rapido.Framework.HTTP.Resilience;
using Rapido.Framework.HTTP.ServiceDiscovery;
using Rapido.Framework.Messaging.RabbitMQ;
using Rapido.Framework.Observability.Logging;
using Rapido.Framework.Observability.Tracing;
using Rapido.Framework.Security;
using Rapido.Framework.Security.Vault;

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
            .AddCorsPolicy(builder.Configuration)
            .AddHttpContextAccessor()
            .AddContexts()
            .AddMemoryCache()
            .AddEndpointsApiExplorer()
            .AddSwaggerDocs(builder.Configuration)
            .AddAuth(builder.Configuration)
            .AddConsul()
            .AddTracing(builder.Configuration);
        
        builder.Services
            .AddHttpClient()
            .AddHttpClientResilience();

        builder.AddSecurity();

        builder.Services
            .AddCommands()
            .AddQueries()
            .AddDomainEvents()
            .AddDispatcher();

        builder.Services
            .AddLogging(builder.Configuration);

        builder.Services
            .AddRabbitMq(builder.Configuration);

        if (builder.Environment.EnvironmentName == "Docker")
        {
            builder.Configuration
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true);
        }

        return builder;
    }

    public static WebApplication UseFramework(this WebApplication app)
    {
        app
            .UseForwardedHeaders()
            .UseCorsPolicy()
            .UseExceptionHandling()
            .UseSwaggerDocs()
            .UseAuthentication()
            .UseRouting()
            .UseAuthorization();

        return app;
    }
}