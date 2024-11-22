using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace Rapido.Framework.HTTP.ServiceDiscovery;

public static class Extensions
{
    private const string HttpClientName = "consul";
    
    public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceDiscovery(o => o.UseConsul());
        services.AddHttpClient(HttpClientName).AddServiceDiscovery();
        services.AddDiscoveryClient();
        
        return services;
    }
}