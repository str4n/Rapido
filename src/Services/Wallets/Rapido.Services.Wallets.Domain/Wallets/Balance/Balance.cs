using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Domain.Wallets.Balance;

public sealed class Balance
{
    public BalanceId Id { get; }
    public WalletId WalletId { get; }
    
    private readonly HashSet<InternalTransfer> _transfers = new();
    public IEnumerable<InternalTransfer> Transfers => _transfers;
    public Amount Amount => CurrentAmount();
    public Currency Currency { get; }
    public bool IsPrimary { get; private set; }
    public DateTime CreatedAt { get; }

    internal Balance(BalanceId id, WalletId walletId, Currency currency, bool isPrimary, DateTime createdAt)
    {
        Id = id;
        WalletId = walletId;
        Currency = currency;
        IsPrimary = isPrimary;
        CreatedAt = createdAt;
    }

    private Balance()
    {
    }

    internal static Balance Create(WalletId walletId, Currency currency, bool isPrimary, DateTime createdAt)
        => new(Guid.NewGuid(), walletId, currency, isPrimary, createdAt);

    internal void AddTransfer(InternalTransfer transfer) => _transfers.Add(transfer);
    
    private Amount CurrentAmount()
        => _transfers.OfType<IncomingInternalTransfer>().Sum(x => x.Amount) - _transfers.OfType<OutgoingInternalTransfer>().Sum(x => x.Amount);
}