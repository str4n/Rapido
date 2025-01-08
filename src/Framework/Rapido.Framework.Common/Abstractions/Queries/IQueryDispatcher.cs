namespace Rapido.Framework.Common.Abstractions.Queries;

public interface IQueryDispatcher
{
    Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) 
        where TQuery : class, IQuery<TResult>;
}