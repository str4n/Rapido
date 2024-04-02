using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Serialization;
using Rapido.Framework.Common.Time;

namespace Rapido.Framework.Common;

public static class Extensions
{
    private const string SectionName = "app";
    public static IServiceCollection AddBaseFeatures(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<IClock, UtcClock>()
            .AddSingleton<IJsonSerializer, NewtonsoftJsonSerializer>()
            .Configure<AppOptions>(configuration.GetSection(SectionName));
    
    public static TOptions BindOptions<TOptions>(this IConfiguration configuration, string sectionName) where TOptions : class, new()
        => BindOptions<TOptions>(configuration.GetSection(sectionName));
    
    public static TOptions BindOptions<TOptions>(this IConfigurationSection section) where TOptions : class, new()
    {
        var options = new TOptions();
        section.Bind(options);

        return options;
    }
}