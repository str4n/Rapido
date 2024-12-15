using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace Rapido.APIGateway.Configuration;

public record ProxyConfig : IProxyConfig
{
    public IReadOnlyList<RouteConfig> Routes { get; }
    public IReadOnlyList<ClusterConfig> Clusters { get; }
    private readonly CancellationTokenSource _changeTokenSource = new();
    public IChangeToken ChangeToken { get; }

    public ProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        Routes = routes;
        Clusters = clusters;
        ChangeToken = new CancellationChangeToken(_changeTokenSource.Token);
    }
}