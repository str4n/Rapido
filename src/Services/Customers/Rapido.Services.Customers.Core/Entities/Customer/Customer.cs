﻿using Rapido.Services.Customers.Core.Entities.Lockout;
using Rapido.Services.Customers.Core.Exceptions;

namespace Rapido.Services.Customers.Core.Entities.Customer;

internal sealed class Customer
{
    public Guid Id { get; }
    public Email Email { get; private set; }
    public Name Name { get; private set; }
    public FullName FullName { get; private set; }
    public Address Address { get; private set; }
    public Nationality Nationality { get; private set; }
    public Identity Identity { get; private set; }
    public CustomerState State { get; private set; }
    public CustomerType Type { get; private set; }
    public CustomerState StateBeforeLockout { get; private set; }
    private readonly List<Lockout.Lockout> _lockouts = new();
    public IEnumerable<Lockout.Lockout> Lockouts => _lockouts;
    public DateTime CreatedAt { get; private set; }
    public DateTime CompletedAt { get; private set; }

    
    public Customer(Guid id, Email email, CustomerType type, DateTime createdAt)
    {
        Id = id;
        Email = email;
        State = CustomerState.NotCompleted;
        Type = type;
        CreatedAt = createdAt;
    }

    private Customer()
    {
    }

    public void Complete(Name name, FullName fullName, Address address, Nationality nationality, Identity identity,
        DateTime completedAt)
    {
        if (State != CustomerState.NotCompleted)
        {
            throw new CustomerNotActiveException();
        }

        Name = name;
        FullName = fullName;
        Address = address;
        Nationality = nationality;
        Identity = identity;
        CompletedAt = completedAt;
        State = CustomerState.Completed;
        StateBeforeLockout = CustomerState.None;
    }
    

    public void Lock(Lockout.Lockout lockout)
    {
        if (State is CustomerState.Locked)
        {
            throw new CannotLockCustomerException();
        }
        
        StateBeforeLockout = State;
        
        _lockouts.Add(lockout);

        State = CustomerState.Locked;
    }

    public void Unlock(DateTime now)
    {
        if (State is not CustomerState.Locked && !Lockouts.Any())
        {
            throw new CannotUnlockCustomerException();
        }

        var activeLockout = _lockouts.Last();

        if (activeLockout is TemporaryLockout tempLockout)
        {
            tempLockout.EndDate = now;
        }

        if (activeLockout is PermanentLockout permLockout)
        {
            permLockout.Active = false;
        }
        
        State = StateBeforeLockout;
    }
}