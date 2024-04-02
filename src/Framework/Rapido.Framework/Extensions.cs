using Microsoft.AspNetCore.Builder;
using Rapido.Framework.Base;
using Rapido.Framework.CQRS;

namespace Rapido.Framework;

public static class Extensions
{
    public static WebApplicationBuilder AddFramework(this WebApplicationBuilder builder)
    {
        builder.Services.AddBaseFeatures(builder.Configuration);

        builder.Services
            .AddCommands()
            .AddQueries()
            .AddDispatcher();

        return builder;
    }
}