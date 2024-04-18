using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Common.Abstractions.DomainEvents;
using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Framework.Common.Dispatchers.Commands;
using Rapido.Framework.Common.Dispatchers.DomainEvents;
using Rapido.Framework.Common.Dispatchers.Queries;

namespace Rapido.Framework.Common.Dispatchers;

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

    public static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        services.Scan(s =>
            s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddSingleton<IDomainEventDispatcher, InMemoryDomainEventDispatcher>();

        return services;
    }

    public static IServiceCollection AddDispatcher(this IServiceCollection services)
        => services.AddSingleton<IDispatcher, InMemoryDispatcher>();
}