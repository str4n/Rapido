using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Attributes;
using Rapido.Framework.Postgres.UnitOfWork;

namespace Rapido.Framework.Postgres.Decorators;

[Decorator]
internal sealed class TransactionalCommandHandlerDecorator<T>(ICommandHandler<T> handler, IUnitOfWork unitOfWork) 
    : ICommandHandler<T> where T : class, ICommand
{
    public Task HandleAsync(T command, CancellationToken cancellationToken)
        => unitOfWork.ExecuteAsync(() => handler.HandleAsync(command, cancellationToken), cancellationToken);
}