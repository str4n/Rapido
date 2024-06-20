using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public abstract class Transfer
{
    public TransferId Id { get; }
    public WalletId WalletId { get; }
    public TransferName Name { get; }
    public TransferMetadata Metadata { get; }
    public Currency Currency { get; }
    public Amount Amount { get; }
    public DateTime CreatedAt { get; }

    protected Transfer(TransferId id, WalletId walletId, TransferName name, 
        Currency currency, Amount amount, DateTime createdAt, TransferMetadata metadata = null)
    {
        Id = id;
        WalletId = walletId;
        Name = name;
        Metadata = metadata;
        Currency = currency;
        Amount = amount;
        CreatedAt = createdAt;
    }

    protected Transfer()
    {
    }
}