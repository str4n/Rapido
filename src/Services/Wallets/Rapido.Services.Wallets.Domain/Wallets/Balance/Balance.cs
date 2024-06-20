using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Domain.Wallets.Balance;

public sealed class Balance
{
    public BalanceId Id { get; }
    public WalletId WalletId { get; }
    public Amount Amount { get; private set; }
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
        Amount = 0;
    }

    private Balance()
    {
    }

    internal static Balance Create(WalletId walletId, Currency currency, bool isPrimary, DateTime createdAt)
        => new(Guid.NewGuid(), walletId, currency, isPrimary, createdAt);

    internal void AddFunds(Amount amount) => Amount += amount;
    internal void DeductFunds(Amount amount) => Amount -= amount;
}