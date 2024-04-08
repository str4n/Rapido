using Rapido.Framework.Common.Abstractions;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Framework.Common.Dispatchers;

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