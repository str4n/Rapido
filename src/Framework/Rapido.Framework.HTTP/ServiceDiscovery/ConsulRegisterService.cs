using System.Net;
using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Rapido.Framework.HTTP.ServiceDiscovery;

public class ConsulRegisterService : IHostedService
{
    private readonly IConsulClient _consulClient;
    private readonly ConsulOptions _options;
    private readonly ILogger<ConsulRegisterService> _logger;
    private readonly Uri _consulUrl;
    private readonly Uri _serviceUrl;
    private readonly string _serviceName;
    private readonly string _serviceId;

    public ConsulRegisterService(IConsulClient consulClient, IOptions<ConsulOptions> options, 
        ILogger<ConsulRegisterService> logger)
    {
        _consulClient = consulClient;
        _options = options.Value;
        _logger = logger;
        _consulUrl = new Uri(options.Value.Url);
        _serviceUrl = new Uri(options.Value.Service.Url);
        _serviceName = options.Value.Service.Name;
        _serviceId = $"{_serviceName}-{Guid.NewGuid():N}";
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var serviceRegistration = new AgentServiceRegistration
        {
            ID = _serviceId,
            Name = _serviceId,
            Address = _serviceUrl.Host,
            Port = _serviceUrl.Port,
            Tags = [_serviceName],
            // Check = new AgentServiceCheck
            // {
            //     HTTP = $"{_serviceUrl}{_options.HealthCheck.Endpoint}",
            //     Interval = _options.HealthCheck.Interval,
            //     DeregisterCriticalServiceAfter = _options.HealthCheck.DeregisterInterval
            // }
        };

        var result = await _consulClient.Agent.ServiceRegister(serviceRegistration, cancellationToken);

        if (result.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation($"{_serviceName} service was registered successfully.");
            return;
        }
        
        _logger.LogError($"There was an error: {result.StatusCode} when registering {_serviceName} service.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        var result = await _consulClient.Agent.ServiceDeregister(_serviceId, cancellationToken);
        
        if (result.StatusCode == HttpStatusCode.OK)
        {
            _logger.LogInformation($"{_serviceName} service was deregistered successfully.");
            return;
        }
        
        _logger.LogError($"There was an error: {result.StatusCode} when deregistering {_serviceName} service.");
    }
}