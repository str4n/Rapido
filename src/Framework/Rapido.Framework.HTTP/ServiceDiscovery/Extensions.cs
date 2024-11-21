using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Consul;

namespace Rapido.Framework.HTTP.ServiceDiscovery;

public static class Extensions
{
    private const string SectionName = "consul";
    private const string HttpClientName = "consul";
    
    public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        // var section = configuration.GetSection(SectionName);
        // var options = section.BindOptions<ConsulOptions>();
        // services.Configure<ConsulOptions>(section);
        //
        // if (options.Enabled)
        // {
        //     services.AddSingleton<IConsulClient>(new ConsulClient(config=>
        //     { 
        //         config.Address = new Uri(options.Url);
        //     }));
        //     
        //     services.AddTransient<ConsulHttpHandler>();
        //     services.AddHostedService<ConsulRegisterService>();
        // }
        //

        services.AddServiceDiscovery(o => o.UseConsul());

        services.AddHttpClient(HttpClientName).AddServiceDiscovery();
        
        
        return services;
    }

    public static IServiceCollection AddConsulHandler(this IServiceCollection services, IConfiguration configuration)
    {
        // var section = configuration.GetSection(SectionName);
        // var options = section.BindOptions<ConsulOptions>();
        //
        // if (!options.Enabled)
        // {
        //     return services;
        // }
        //
        // services
        //     .AddHttpClient(HttpClientName)
        //     .AddHttpMessageHandler<ConsulHttpHandler>();
        //
        return services;
    }
}