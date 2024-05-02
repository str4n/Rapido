using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common;
using Rapido.Framework.Postgres;

namespace Rapido.Framework.Health.HealthChecks;

public static class Extensions
{
    private const string PostgresSectionName = "postgres";
    
    public static IServiceCollection AddHealth(this IServiceCollection services, IConfiguration configuration)
    {
        var postgresOptions = configuration.BindOptions<PostgresOptions>(PostgresSectionName);

        services
            .AddHealthChecks()
            .AddNpgSql(postgresOptions.ConnectionString);
        
        return services;
    }

    public static IEndpointRouteBuilder UseHealth(this IEndpointRouteBuilder app)
    {
        app.MapHealthChecks("health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        
        return app;
    }
}