namespace Rapido.Framework.Common.Abstractions.DomainEvents;

public interface IDomainEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
}