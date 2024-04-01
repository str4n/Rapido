using Microsoft.Extensions.Configuration;

namespace Rapido.Framework;

public static class Extensions
{
    public static TOptions BindOptions<TOptions>(this IConfiguration configuration, string sectionName) where TOptions : class, new()
        => BindOptions<TOptions>(configuration.GetSection(sectionName));
    
    public static TOptions BindOptions<TOptions>(this IConfigurationSection section) where TOptions : class, new()
    {
        var options = new TOptions();
        section.Bind(options);

        return options;
    }
}