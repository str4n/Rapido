namespace Rapido.Framework.Common.Abstractions.Queries;

public interface IQueryDispatcher
{
    Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query);
    Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>;
}