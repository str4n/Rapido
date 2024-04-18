namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public sealed record TransferId(Guid Value)
{
    public TransferId() : this(Guid.NewGuid())
    {
    }

    public static TransferId Create() => new(Guid.NewGuid());
    
    public static implicit operator Guid(TransferId id) => id.Value;
    
    public static implicit operator TransferId(Guid id) => new(id);
    
    public override string ToString() => Value.ToString();
}