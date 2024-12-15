namespace Rapido.APIGateway.Configuration.Loader;

public interface IProxyConfigLoader
{
    Task ReloadAsync();
}