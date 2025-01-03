using Rapido.Services.Wallets.Domain.Wallets.Balance;
using Rapido.Services.Wallets.Domain.Wallets.Money;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

// Represents transfer within wallet balance including exchange rate if needed
public abstract class InternalTransfer
{
    public TransferId Id { get; }
    public TransactionId TransactionId { get; }
    public BalanceId BalanceId { get; }
    public TransferMetadata Metadata { get; }
    public Currency Currency { get; }
    public Amount Amount { get; }
    public ExchangeRate ExchangeRate { get; }
    public DateTime CreatedAt { get; }

    protected InternalTransfer(TransferId id, TransactionId transactionId, BalanceId balanceId, Currency currency, 
        Amount amount, DateTime createdAt, TransferMetadata metadata = null, ExchangeRate exchangeRate = null)
    {
        Id = id;
        TransactionId = transactionId;
        BalanceId = balanceId;
        Metadata = metadata;
        Currency = currency;
        Amount = amount;
        ExchangeRate = exchangeRate;
        CreatedAt = createdAt;
    }
    
    protected InternalTransfer()
    {
    }
}