using Steeltoe.Discovery;
using Steeltoe.Discovery.Consul.Discovery;
using Yarp.ReverseProxy.Configuration;

namespace Rapido.APIGateway.Configuration.Provider;

internal sealed class ServiceDiscoveryConfigConfigProvider : IServiceDiscoveryConfigProvider
{
    private readonly InMemoryConfigProvider _configProvider;
    private readonly IConfigValidator _configValidator;
    private readonly IConsulDiscoveryClient _discoveryClient;
    
    public ServiceDiscoveryConfigConfigProvider(IDiscoveryClient discoveryClient, 
        InMemoryConfigProvider configProvider, IConfigValidator configValidator)
    {
        _configProvider = configProvider;
        _configValidator = configValidator;
        _discoveryClient = discoveryClient as IConsulDiscoveryClient;
    }
    
    public async Task ReloadAsync()
    {
        var config = await CreateConfig();
        
        _configProvider.Update(config.Routes, config.Clusters);
    }

    private async Task<ProxyConfig> CreateConfig()
    {
        var services = _discoveryClient.GetAllInstances();

        var routes = new List<RouteConfig>();
        var clusters = new List<ClusterConfig>();

        foreach (var service in services.Where(x => x.ServiceId != "consul"))
        {
            var routeConfig = new RouteConfig
            {
                RouteId = $"{service.ServiceId}-route",
                ClusterId = $"{service.ServiceId}-cluster",
                Match = new RouteMatch
                {
                    Path = $"{service.ServiceId}/{{**catchall}}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new()
                    {
                        ["PathPattern"] = "{**catchall}"
                    }
                }
            };

            var clusterConfig = new ClusterConfig
            {
                ClusterId = $"{service.ServiceId}-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    {
                        service.ServiceId, new DestinationConfig
                        {
                            Address = service.Uri.ToString()
                        }
                    }
                }
            };
            
            //TODO: Check for duplicates

            if ((await _configValidator.ValidateRouteAsync(routeConfig)).Count == 0)
            {
                routes.Add(routeConfig);
            }
            
            if ((await _configValidator.ValidateClusterAsync(clusterConfig)).Count == 0)
            {
                clusters.Add(clusterConfig);
            }
        }

        await _discoveryClient.ShutdownAsync();

        return new ProxyConfig(routes, clusters);
    }
}