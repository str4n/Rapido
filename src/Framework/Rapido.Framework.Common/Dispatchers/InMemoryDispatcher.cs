using Rapido.Framework.Common.Abstractions;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Framework.Common.Dispatchers;

internal sealed class InMemoryDispatcher(
    IQueryDispatcher queryDispatcher, 
    ICommandDispatcher commandDispatcher)
    : IDispatcher
{
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand
        => await commandDispatcher.DispatchAsync(command, cancellationToken);

    public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => await queryDispatcher.DispatchAsync(query, cancellationToken);
}