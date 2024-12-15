namespace Rapido.APIGateway.Configuration.Builder;

public interface IProxyConfigBuilder
{
    Task<ProxyConfig> BuildAsync();
}