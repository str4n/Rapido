using Rapido.APIGateway.Configuration.Provider;
using Rapido.Framework.Common;
using Yarp.ReverseProxy.Configuration;

namespace Rapido.APIGateway.Configuration;

public static class Extensions
{
    private const string SectionName = "reverseProxy";
    
    public static IReverseProxyBuilder LoadConfig(this IReverseProxyBuilder builder, IConfiguration configuration)
    {
        var options = configuration.BindOptions<LoadOptions>(SectionName);

        if (options.EnableAutoDiscovery)
        {
            builder.LoadFromConsul();
        }
        else
        {
            builder.LoadFromConfig(configuration.GetSection(SectionName));
        }

        return builder;
    }
    
    private static IReverseProxyBuilder LoadFromConsul(this IReverseProxyBuilder builder)
    {
        builder.LoadFromMemory(default, default);

        builder.Services.AddSingleton<IServiceDiscoveryConfigProvider, ServiceDiscoveryConfigConfigProvider>();
        builder.Services.AddHostedService<ServiceDiscoveryConfigUpdater>();

        return builder;
    }
}