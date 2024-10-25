using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Aggregate;
using Rapido.Services.Wallets.Domain.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Domain.Wallets.Wallet;

public sealed class Wallet : AggregateRoot<WalletId>
{
    public OwnerId OwnerId { get; }

    private readonly HashSet<Transfer.Transfer> _transfers = new();
    private readonly HashSet<Balance.Balance> _balances = new();
    public IEnumerable<Transfer.Transfer> Transfers => _transfers;
    public IEnumerable<Balance.Balance> Balances => _balances;
    public DateTime CreatedAt { get; }
    internal Wallet(WalletId id, OwnerId ownerId, Currency currency, DateTime createdAt)
    {
        Id = new(id);
        OwnerId = ownerId;
        CreatedAt = createdAt;

        var balance = Balance.Balance.Create(id, currency, true, createdAt);
        _balances.Add(balance);
    }

    private Wallet()
    {
    }

    public static Wallet Create(OwnerId ownerId, Currency currency, DateTime createdAt)
        => new Wallet(Guid.NewGuid(), ownerId, currency, createdAt);

    public void AddBalance(Currency currency, DateTime now)
    {
        if (BalanceExists(currency))
        {
            throw new BalanceAlreadyExistsException(currency);
        }
        
        var balance = Balance.Balance.Create(Id, currency, false, now);

        _balances.Add(balance);
        IncrementVersion();
    }

    public IncomingTransfer AddFunds(TransferName name, Amount amount, 
        Currency currency, ExchangeRate exchangeRate, DateTime now)
    {
        var transferId = new TransferId();

        var balance = GetPrimaryBalance();

        if (balance is null)
        {
            throw new BalanceNotFoundException();
        }
        
        if (amount <= Amount.Zero)
        {
            throw new InvalidTransferAmountException(amount);
        }

        if (exchangeRate.From != currency)
        {
            throw new InvalidExchangeException("Exchange rate from currency doesn't equal transfer currency.");
        }

        if (exchangeRate.To != balance.Currency)
        {
            throw new InvalidExchangeException("Exchange rate to currency doesn't equal primary currency.");
        }

        var exchangedAmount = amount * exchangeRate.Rate;

        var transfer = new IncomingTransfer(transferId, Id, name, currency, amount, now, GetMetadata(transferId, Id));

        _transfers.Add(transfer);
        balance.AddFunds(exchangedAmount);
        
        IncrementVersion();

        return transfer;
    }

    public OutgoingTransfer DeductFunds(TransferName name, Amount amount, Currency currency,
        List<ExchangeRate> exchangeRates, DateTime now)
    {
        var transferId = new TransferId();
        
        if (amount <= Amount.Zero)
        {
            throw new InvalidTransferAmountException(amount);
        }
        
        var remainingAmount = amount.Value;

        foreach (var balance in _balances
                     .OrderByDescending(x => x.IsPrimary)
                     .TakeWhile(_ => !(remainingAmount <= 0)))
            
        {
            DeductFundsFromBalance(balance);
        }

        if (remainingAmount > 0)
        {
            throw new InsufficientFundsException(Id);
        }
        
        var transfer = new OutgoingTransfer(transferId, Id, name, currency, amount, now, GetMetadata(transferId, Id));
        _transfers.Add(transfer);
        
        IncrementVersion();

        return transfer;
        
        void DeductFundsFromBalance(Balance.Balance balance)
        {
            var exchangeRate = GetExchangeRate(balance.Currency, currency);
            var exchangedAmountFromBalance = balance.Amount * exchangeRate;

            if (exchangedAmountFromBalance <= Amount.Zero)
            {
                return;
            }
            
            if (exchangedAmountFromBalance >= remainingAmount)
            {
                var amountToDeduct = remainingAmount / exchangeRate;
                balance.DeductFunds(amountToDeduct);
                remainingAmount = 0;
            }
            else
            {
                var amountToDeduct = balance.Amount;
                balance.DeductFunds(amountToDeduct);
                remainingAmount -= exchangedAmountFromBalance;
            }
        }
        
        double GetExchangeRate(Currency from, Currency to) => exchangeRates
            .Single(x => x.From == from && x.To == to).Rate;
    }
    
    public Amount GetFunds(Currency currency)
    {
        var balance = GetBalance(currency);

        if (balance is null)
        {
            throw new BalanceNotFoundException();
        }

        return balance.Amount;
    }

    public Amount GetTotalFunds(List<ExchangeRate> exchangeRates)
    {
        var totalAmountInPrimaryCurrency = Amount.Zero;

        foreach (var balance in _balances)
        {
            var primaryCurrency = GetPrimaryCurrency();
            var exchangeRate = exchangeRates.Single(x => x.From == balance.Currency && x.To == primaryCurrency).Rate;

            totalAmountInPrimaryCurrency += balance.Amount * exchangeRate;
        }

        return totalAmountInPrimaryCurrency;
    }
    
    public Currency GetPrimaryCurrency() 
        => _balances.SingleOrDefault(x => x.IsPrimary)?.Currency;

    private Balance.Balance GetBalance(Currency currency) 
        => _balances.SingleOrDefault(x => x.Currency == currency);

    private Balance.Balance GetPrimaryBalance() 
        => _balances.SingleOrDefault(x => x.IsPrimary);
    
    private bool BalanceExists(Currency currency) 
        => _balances.Any(x => x.Currency == currency);
    
    private static TransferMetadata GetMetadata(TransferId transferId, WalletId walletId)
        => new($"{{\"transferId\": \"{transferId}\", \"walletId\": \"{walletId}\"}}");
}