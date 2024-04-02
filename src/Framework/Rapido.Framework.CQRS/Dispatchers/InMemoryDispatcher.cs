using Rapido.Framework.CQRS.Commands;
using Rapido.Framework.CQRS.Queries;

namespace Rapido.Framework.CQRS.Dispatchers;

internal sealed class InMemoryDispatcher : IDispatcher
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public InMemoryDispatcher(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }


    public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        => await _commandDispatcher.DispatchAsync(command);

    public async Task<TResult> DispatchAsync<TResult>(IQuery<TResult> query)
        => await _queryDispatcher.DispatchAsync(query);
}