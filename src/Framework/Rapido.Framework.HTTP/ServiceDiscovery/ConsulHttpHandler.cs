using Consul;
using Microsoft.Extensions.Options;
using Rapido.Framework.HTTP.ServiceDiscovery.Exceptions;

namespace Rapido.Framework.HTTP.ServiceDiscovery;

internal sealed class ConsulHttpHandler : DelegatingHandler
{
    private readonly IConsulClient _client;
    private readonly bool _enabled;

    public ConsulHttpHandler(IConsulClient client, IOptions<ConsulOptions> options)
    {
        _client = client;
        _enabled = options.Value.Enabled;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!_enabled)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        if (request.RequestUri is null)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var serviceName = request.RequestUri.Host;
        
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var services = (await _client.Agent.Services(cancellationToken))
            .Response.Where(x => x.Key.Contains(serviceName)).ToDictionary();
        
        if (services.Count == 0)
        {
            throw new ServiceNotFoundException(serviceName);
        }
        
        var service = services.First().Value;
        
        var uriBuilder = new UriBuilder(request.RequestUri)
        {
            Host = service.Address,
            Port = service.Port
        };
        
        request.RequestUri = uriBuilder.Uri;

        return await base.SendAsync(request, cancellationToken);
    }
}