namespace Rapido.Services.Wallets.Domain.Owners.Owner;

public abstract class Owner
{
    public OwnerId Id { get; }
    public OwnerName Name { get; private set; }
    public OwnerState State { get; protected set; }
    public DateTime CreatedAt { get; private set; }

    public Owner(OwnerName name, DateTime createdAt) : this(new OwnerId(), name, createdAt)
    {
    }

    public Owner(OwnerId id, OwnerName name, DateTime createdAt)
    {
        Id = id;
        Name = name;
        CreatedAt = createdAt;
        State = OwnerState.Active;
    }
    
    public void Lock() => State = OwnerState.Locked;

    public void Unlock() => State = OwnerState.Active;
}