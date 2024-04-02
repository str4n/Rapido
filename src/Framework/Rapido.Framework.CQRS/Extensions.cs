using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.CQRS.Commands;
using Rapido.Framework.CQRS.Commands.Dispatchers;
using Rapido.Framework.CQRS.Dispatchers;
using Rapido.Framework.CQRS.Queries;
using Rapido.Framework.CQRS.Queries.Dispatchers;

namespace Rapido.Framework.CQRS;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddSingleton<ICommandDispatcher, InMemoryCommandDispatcher>();

        return services;
    }

    public static IServiceCollection AddQueries(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddSingleton<IQueryDispatcher, InMemoryQueryDispatcher>();

        return services;
    }

    public static IServiceCollection AddDispatcher(this IServiceCollection services)
        => services.AddSingleton<IDispatcher, InMemoryDispatcher>();
}