using Rapido.APIGateway.Configuration.Loader;
using Yarp.ReverseProxy.Configuration;

namespace Rapido.APIGateway.Configuration;

public class ServiceDiscoveryConfigUpdater(
    IProxyConfigLoader loader) 
    : IHostedService, IDisposable
{
    private Timer _timer;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        loader.ReloadAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    public void Dispose()
    {
        _timer.Dispose();
    }
}