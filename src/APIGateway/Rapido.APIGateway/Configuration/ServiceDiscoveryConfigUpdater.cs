using Rapido.APIGateway.Configuration.Provider;
using Yarp.ReverseProxy.Configuration;

namespace Rapido.APIGateway.Configuration;

public class ServiceDiscoveryConfigUpdater(
    IServiceDiscoveryConfigProvider provider) 
    : IHostedService, IDisposable
{
    private Timer _timer;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        provider.ReloadAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    public void Dispose()
    {
        _timer.Dispose();
    }
}