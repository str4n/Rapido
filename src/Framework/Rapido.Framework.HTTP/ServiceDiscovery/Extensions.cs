using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common;

namespace Rapido.Framework.HTTP.ServiceDiscovery;

public static class Extensions
{
    public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("consul");
        var options = section.BindOptions<ConsulOptions>();
        services.Configure<ConsulOptions>(section);

        if (options.Enabled)
        {
            services.AddSingleton<IConsulClient>(new ConsulClient(config=>
            { 
                config.Address = new Uri(options.Url);
            }));

            services.AddHostedService<ConsulRegisterService>();
        }

        return services;
    }
}