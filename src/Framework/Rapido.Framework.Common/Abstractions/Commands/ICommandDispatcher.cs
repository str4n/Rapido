namespace Rapido.Framework.Common.Abstractions.Commands;

public interface ICommandDispatcher
{
    Task DispatchAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
}