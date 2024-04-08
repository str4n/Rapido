using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Framework.Common.Dispatchers.Commands;

internal sealed class InMemoryCommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public InMemoryCommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        await handler.HandleAsync(command);
    }
}