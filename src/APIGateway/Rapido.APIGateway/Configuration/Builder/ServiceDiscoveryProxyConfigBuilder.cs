using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;
using Steeltoe.Discovery.Consul.Discovery;
using Yarp.ReverseProxy.Configuration;

namespace Rapido.APIGateway.Configuration.Builder;

internal sealed class ServiceDiscoveryProxyConfigBuilder(
    IDiscoveryClient discoveryClient,
    IConfigValidator configValidator,
    ILogger<ServiceDiscoveryProxyConfigBuilder> logger) : IProxyConfigBuilder
{
    private readonly IConsulDiscoveryClient _discoveryClient = discoveryClient as IConsulDiscoveryClient;
    
    public async Task<ProxyConfig> BuildAsync()
    {
        var routes = await BuildRoutes();
        var clusters = await BuildClusters();

        return new ProxyConfig(routes.ToList(), clusters.ToList());
    }

    private async Task<IEnumerable<RouteConfig>> BuildRoutes()
    {
        logger.LogInformation("(ProxyConfigBuilder) Building proxy routes...");
        
        var services = _discoveryClient.GetServices().Where(x => x is not "consul" and not "api-gateway");
        
        var routes = new List<RouteConfig>();

        foreach (var service in services)
        {
            var route = new RouteConfig
            {
                RouteId = GenerateRouteIdByServiceName(service),
                ClusterId = GenerateClusterIdByServiceName(service),
                Match = new RouteMatch
                {
                    Path = $"{service}/{{**catchall}}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new()
                    {
                        ["PathPattern"] = "{**catchall}"
                    }
                }
            };

            if (!await IsRouteValidAsync(route, service))
            {
                continue;
            }

            if (IsRouteAlreadyAdded(service))
            {
                continue;
            }

            logger.LogInformation("(ProxyConfigBuilder) Created route for {service}.", service);
            
            routes.Add(route);
        }
        
        return routes;
        
        bool IsRouteAlreadyAdded(string service) 
            => routes.Any(x => x.RouteId == GenerateRouteIdByServiceName(service));

        async Task<bool> IsRouteValidAsync(RouteConfig routeConfig, string service)
        {
            var errors = await configValidator.ValidateRouteAsync(routeConfig);

            if (errors.Any())
            {
                logger.LogError("(ProxyConfigBuilder) Errors found when trying to generate route for {Service}", service);
                errors.ToList().ForEach(err => logger.LogError(err, $"{service} route validation error"));

                return false;
            }

            return true;
        }
    }

    private async Task<IEnumerable<ClusterConfig>> BuildClusters()
    {
        logger.LogInformation("(ProxyConfigBuilder) Building proxy clusters...");
        
        var services = _discoveryClient.GetServices().Where(x => x is not "consul" and not "api-gateway");
        
        var clusters = new List<ClusterConfig>();

        foreach (var service in services)
        {
            var cluster = new ClusterConfig
            {
                ClusterId = GenerateClusterIdByServiceName(service)
            };
            
            var instances = _discoveryClient.GetInstances(service);

            var destinations = new Dictionary<string, DestinationConfig>();
            
            foreach (var instance in instances)
            {
                var destination = new DestinationConfig
                {
                    Address = instance.Uri.ToString()
                };
                
                destinations.Add($"{service}-{Guid.NewGuid():N}", destination);
            }

            var clusterWithDestinations = cluster with { Destinations = destinations };

            if (!await IsClusterValidAsync(clusterWithDestinations, service))
            {
                continue;
            }
            
            logger.LogInformation("(ProxyConfigBuilder) Created cluster for {service} with {destinationCount} destinations.", service, destinations.Count);
            
            clusters.Add(clusterWithDestinations);
        }
        
        return clusters;
            
        async Task<bool> IsClusterValidAsync(ClusterConfig clusterConfig, string service)
        {
            var errors = await configValidator.ValidateClusterAsync(clusterConfig);

            if (errors.Any())
            {
                logger.LogError("(ProxyConfigBuilder) Errors found when trying to generate cluster for {Service}", service);
                errors.ToList().ForEach(err => logger.LogError(err, $"{service} cluster validation error"));

                return false;
            }

            return true;
        }
    }

    private static string GenerateRouteIdByServiceName(string service) => $"{service}-route";
    private static string GenerateClusterIdByServiceName(string service) => $"{service}-cluster";
}