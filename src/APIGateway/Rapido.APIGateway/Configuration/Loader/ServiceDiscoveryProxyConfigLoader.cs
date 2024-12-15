using Rapido.APIGateway.Configuration.Builder;
using Yarp.ReverseProxy.Configuration;

namespace Rapido.APIGateway.Configuration.Loader;

internal sealed class ServiceDiscoveryProxyConfigLoader(
    InMemoryConfigProvider configProvider, 
    IProxyConfigBuilder configBuilder) : IProxyConfigLoader
{
    public async Task ReloadAsync()
    {
        var config = await configBuilder.BuildAsync();
        
        configProvider.Update(config.Routes, config.Clusters);
    }
}