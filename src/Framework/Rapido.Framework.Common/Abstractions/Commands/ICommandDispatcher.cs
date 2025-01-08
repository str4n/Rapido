namespace Rapido.Framework.Common.Abstractions.Commands;

public interface ICommandDispatcher
{
    Task DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : class, ICommand;
}