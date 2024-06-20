namespace Rapido.Services.Wallets.Domain.Wallets.Balance;

public sealed record BalanceId(Guid Value)
{
    public BalanceId() : this(Guid.NewGuid())
    {
    }

    public static BalanceId Create() => new(Guid.NewGuid());
    
    public static implicit operator Guid(BalanceId id) => id.Value;
    
    public static implicit operator BalanceId(Guid id) => new(id);
    
    public override string ToString() => Value.ToString();
}