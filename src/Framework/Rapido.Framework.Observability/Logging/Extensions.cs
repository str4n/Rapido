using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Framework.Observability.Logging.Decorators;
using Serilog;
using Serilog.Events;

namespace Rapido.Framework.Observability.Logging;

public static class Extensions
{
    private const string SerilogSectionName = "logger";
    private const string AppSectionName = "app";

    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SerilogOptions>(configuration.GetSection(SerilogSectionName));
        
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));

        return services;
    }

    public static IHostBuilder UseLogging(this IHostBuilder host, IConfiguration configuration)
    {
        var serilogOptions = configuration.BindOptions<SerilogOptions>(SerilogSectionName);
        var appOptions = configuration.BindOptions<AppOptions>(AppSectionName);

        host.UseSerilog((ctx, cfg) =>
        {
            var level = GetLogEventLevel(serilogOptions.Level);
            
            cfg.Enrich.FromLogContext()
                .MinimumLevel.Is(level)
                .Enrich.WithProperty("Environment", ctx.HostingEnvironment.EnvironmentName)
                .Enrich.WithProperty("Application", appOptions.Name)
                .Enrich.WithProperty("Version", appOptions.Version);
            
            if (serilogOptions.Seq.Enabled)
            {
                cfg.WriteTo.Seq(serilogOptions.Seq.ConnectionString);
            }

            if (serilogOptions.Console.Enabled)
            {
                cfg.WriteTo.Console(outputTemplate: serilogOptions.Console.Template);
            }
        });

        return host;
    }
    
    private static LogEventLevel GetLogEventLevel(string level)
        => Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
            ? logLevel
            : LogEventLevel.Information;
}