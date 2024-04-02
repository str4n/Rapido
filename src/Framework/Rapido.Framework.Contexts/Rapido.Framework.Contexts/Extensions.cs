using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Contexts.Factory;

namespace Rapido.Framework.Contexts;

public static class Extensions
{
    public static IServiceCollection AddContexts(this IServiceCollection services)
    {
        services.AddSingleton<IContextFactory, ContextFactory>();
        services.AddTransient(sp => sp.GetRequiredService<IContextFactory>().Create());

        return services;
    }
}