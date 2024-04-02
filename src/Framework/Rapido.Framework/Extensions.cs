using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Api.Exceptions;
using Rapido.Framework.Auth;
using Rapido.Framework.Base;
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
            .UseAuthentication()
            .UseRouting()
            .UseAuthorization();

        return app;
    }
}