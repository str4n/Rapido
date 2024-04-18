namespace Rapido.Services.Wallets.Domain.Wallets.Wallet;

public sealed record WalletId(Guid Value)
{
    public WalletId() : this(Guid.NewGuid())
    {
    }

    public static WalletId Create() => new(Guid.NewGuid());
    
    public static implicit operator Guid(WalletId id) => id.Value;
    
    public static implicit operator WalletId(Guid id) => new(id);
    
    public override string ToString() => Value.ToString();
}