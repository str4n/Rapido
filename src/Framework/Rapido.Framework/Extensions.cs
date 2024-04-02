﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Api.Exceptions;
using Rapido.Framework.Api.Swagger;
using Rapido.Framework.Auth;
using Rapido.Framework.Base;
using Rapido.Framework.Contexts;
using Rapido.Framework.CQRS;

namespace Rapido.Framework;

public static class Extensions
{
    public static WebApplicationBuilder AddFramework(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddExceptionHandling()
            .AddBaseFeatures(builder.Configuration)
            .AddHttpContextAccessor()
            .AddContexts()
            .AddMemoryCache()
            .AddEndpointsApiExplorer()
            .AddSwaggerDocs(builder.Configuration)
            .AddAuth(builder.Configuration);

        builder.Services
            .AddCommands()
            .AddQueries()
            .AddDispatcher();

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