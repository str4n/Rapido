namespace Rapido.APIGateway.Configuration.Provider;

public interface IServiceDiscoveryConfigProvider
{
    Task ReloadAsync();
}