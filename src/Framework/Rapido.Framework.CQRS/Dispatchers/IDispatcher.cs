using Rapido.Framework.CQRS.Commands;
using Rapido.Framework.CQRS.Queries;

namespace Rapido.Framework.CQRS.Dispatchers;

public interface IDispatcher
{
    Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
    Task DispatchAsync<TResult>(IQuery<TResult> query);
}