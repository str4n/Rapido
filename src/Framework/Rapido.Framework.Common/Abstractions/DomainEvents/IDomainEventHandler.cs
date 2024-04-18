namespace Rapido.Framework.Common.Abstractions.DomainEvents;

public interface IDomainEventHandler<in TEvent> where TEvent : class, IDomainEvent
{
    Task HandleAsync(TEvent @event);
}