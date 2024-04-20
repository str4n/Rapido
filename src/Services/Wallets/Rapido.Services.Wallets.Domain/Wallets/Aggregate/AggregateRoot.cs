﻿using Rapido.Framework.Common.Abstractions.DomainEvents;

namespace Rapido.Services.Wallets.Domain.Wallets.Aggregate;

public abstract class AggregateRoot<T>
{
    public T Id { get; protected set; }
    
    private readonly List<IDomainEvent> _events = new();
    public IEnumerable<IDomainEvent> Events => _events;
    
    public int Version { get; protected set; }
    private bool _versionIncremented = false;

    protected void AddEvent(IDomainEvent @event)
    {
        if (!_events.Any() && !_versionIncremented)
        {
            Version++;
            _versionIncremented = true;
        }
        
        _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();

    protected void IncrementVersion()
    {
        if (_versionIncremented)
        {
            return;
        }

        Version++;
        _versionIncremented = true;
    }
}

public abstract class AggregateRoot : AggregateRoot<AggregateId>
{
}