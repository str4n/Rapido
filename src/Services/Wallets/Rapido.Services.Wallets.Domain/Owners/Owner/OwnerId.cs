namespace Rapido.Services.Wallets.Domain.Owners.Owner;

public record OwnerId(Guid Value)
{
    public OwnerId() : this(Guid.NewGuid())
    {
    }

    public static OwnerId Create() => new(Guid.NewGuid());
    
    public static implicit operator Guid(OwnerId id) => id.Value;
    
    public static implicit operator OwnerId(Guid id) => new(id);
    
    public override string ToString() => Value.ToString();
}