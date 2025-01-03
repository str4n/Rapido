using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public abstract class Transfer
{
    public TransferId Id { get; }
    public TransactionId TransactionId { get; }
    public WalletId WalletId { get; }
    public TransferName Name { get; }
    private readonly HashSet<InternalTransfer> _subTransfers = new();
    public IEnumerable<InternalTransfer> SubTransfers => _subTransfers;
    public TransferMetadata Metadata { get; }
    public Currency Currency { get; }
    public Amount Amount { get; }
    public DateTime CreatedAt { get; }

    protected Transfer(TransferId id, TransactionId transactionId, WalletId walletId, TransferName name, IEnumerable<InternalTransfer> subTransfers,
        Currency currency, Amount amount, DateTime createdAt, TransferMetadata metadata = null)
    {
        Id = id;
        TransactionId = transactionId;
        WalletId = walletId;
        Name = name;
        _subTransfers = subTransfers.ToHashSet();
        Metadata = metadata;
        Currency = currency;
        Amount = amount;
        CreatedAt = createdAt;
    }

    protected Transfer()
    {
    }
}