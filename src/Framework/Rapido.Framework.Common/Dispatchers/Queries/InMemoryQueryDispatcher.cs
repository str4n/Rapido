using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Framework.Common.Dispatchers.Queries;

internal sealed class InMemoryQueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);
        
        var result = await ((Task<TResult>) handlerType
            .GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))
            ?.Invoke(handler, [query, cancellationToken]))!;

        return result;
    }

    public async Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) 
        where TQuery : class, IQuery<TResult>
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();

        return await handler.HandleAsync(query, cancellationToken);
    }
}