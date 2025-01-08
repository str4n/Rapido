using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Framework.Common.Abstractions.Dispatchers;

public interface IDispatcher
{
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand;
    Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}