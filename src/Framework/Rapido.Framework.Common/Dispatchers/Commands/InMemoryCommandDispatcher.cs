using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Framework.Common.Dispatchers.Commands;

internal sealed class InMemoryCommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public async Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
        where TCommand : class, ICommand
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        await handler.HandleAsync(command, cancellationToken);
    }
}