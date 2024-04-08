using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Framework.Common.Dispatchers.Queries;

internal sealed class InMemoryQueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public InMemoryQueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        
        var result = await (Task<TResult>) handlerType
            .GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))
            ?.Invoke(handler, new[] {query});

        return result;
    }

    public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();

        return await handler.HandleAsync(query);
    }
}