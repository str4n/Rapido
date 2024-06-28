using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Rapido.Framework.Common;

namespace Rapido.Framework.Observability.Tracing;

public static class Extensions
{
    private const string TracingSectionName = "tracing";
    private const string AppSectionName = "app";
    
    public static IServiceCollection AddTracing(this IServiceCollection services, IConfiguration configuration)
    {
        var tracingOptions = configuration.BindOptions<TracingOptions>(TracingSectionName);

        if (!tracingOptions.Enabled)
        {
            return services;
        }

        var appOptions = configuration.BindOptions<AppOptions>(AppSectionName);
        var appName = appOptions.Name.Replace(" ", ".");

        services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(appName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation(o => o.SetDbStatementForText = true)
                    .AddSource(MassTransit.Logging.DiagnosticHeaders.DefaultListenerName);

                tracing.AddOtlpExporter();
            });

        return services;
    }
}