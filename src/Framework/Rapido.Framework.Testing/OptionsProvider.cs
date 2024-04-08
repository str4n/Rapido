using Microsoft.Extensions.Configuration;
using Rapido.Framework.Common;

namespace Rapido.Framework.Testing;

public sealed class OptionsProvider
{
    private readonly IConfigurationRoot _configuration;

    public OptionsProvider(string settingsPath = "appsettings.test.json")
    {
        _configuration = GetConfigurationRoot(settingsPath);
    }
    
    public TOptions GetOptions<TOptions>(string sectionName) where TOptions : class, new()
        => _configuration.BindOptions<TOptions>(sectionName);
    
    private static IConfigurationRoot GetConfigurationRoot(string settingsPath)
        => new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile(settingsPath, true)
            .AddEnvironmentVariables()
            .Build();
}