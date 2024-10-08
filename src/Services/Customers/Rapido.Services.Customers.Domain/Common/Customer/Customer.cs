using Rapido.Services.Customers.Domain.Common.Exceptions;
using Rapido.Services.Customers.Domain.Common.Lockout;

namespace Rapido.Services.Customers.Domain.Common.Customer;

public abstract class Customer
{
    public Guid Id { get; }
    public Email Email { get; private set; }
    public Name Name { get; protected set; }
    public Address Address { get; protected set; }
    public Nationality Nationality { get; protected set; }
    public bool IsCompleted { get; protected set; } 
    public bool IsLocked { get; private set; }
    private readonly List<Lockout.Lockout> _lockouts = new();
    public IEnumerable<Lockout.Lockout> Lockouts => _lockouts;
    public DateTime CreatedAt { get; private set; }
    public DateTime CompletedAt { get; protected set; }

    
    public Customer(Guid id, Email email, DateTime createdAt)
    {
        Id = id;
        Email = email;
        CreatedAt = createdAt;
    }

    private Customer()
    {
    }

    public void ChangeAddress(Address address) 
        => Address = address;

    public void ChangeNationality(Nationality nationality)
        => Nationality = nationality;

    public void Lock(Lockout.Lockout lockout)
    {
        if (IsLocked)
        {
            throw new CannotLockCustomerException();
        }
        
        _lockouts.Add(lockout);
        IsLocked = true;
    }

    public void Unlock(DateTime now)
    {
        if (!IsLocked && !Lockouts.Any())
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

        IsLocked = false;
    }
}