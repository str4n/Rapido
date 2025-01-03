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
        Currency currency, List<ExchangeRate> exchangeRates, DateTime now)
    {
        var transferId = new TransferId();
        var transactionId = new TransactionId();

        // Gets balance in transfer currency, if doesn't exists gets primary balance
        var balance = GetBalance(currency) ?? GetPrimaryBalance();
        
        if (balance is null)
        {
            throw new BalanceNotFoundException();
        }

        var exchangeRate = GetExchangeRate(currency, balance.Currency, exchangeRates);

        if (amount <= Amount.Zero)
        {
            throw new InvalidTransferAmountException(amount);
        }
        
        var exchangedAmount = amount * exchangeRate.Rate;

        var internalTransfer = new IncomingInternalTransfer(transferId, transactionId, balance.Id, balance.Currency, 
            exchangedAmount, now, GetMetadata(transactionId, Id), exchangeRate);
        
        var internalTransfers = new List<InternalTransfer>
        {
            internalTransfer
        };
        
        var transfer = new IncomingTransfer(new TransferId(), transactionId, Id, name, internalTransfers, currency, amount, now, GetMetadata(transactionId, Id));

        _transfers.Add(transfer);
        balance.AddTransfer(internalTransfer);
        
        IncrementVersion();

        return transfer;
    }

    public OutgoingTransfer DeductFunds(TransferName name, Amount amount, Currency currency,
        List<ExchangeRate> exchangeRates, DateTime now)
    {
        var transferId = new TransferId();
        var transactionId = new TransactionId();
        
        if (amount <= Amount.Zero)
        {
            throw new InvalidTransferAmountException(amount);
        }
        
        var remainingAmount = amount.Value;
        
        // This method deducts funds from the balance in the transaction currency.
        // If insufficient, it automatically uses the primary balance, and then any remaining available balances,
        // performing currency exchange if necessary.

        var subTransfers = _balances.OrderBy(x =>
            {
                if (x.Currency == currency) return 0;
                
                return x.IsPrimary ? 1 : 2;
            })
            .TakeWhile(_ => !(remainingAmount <= 0))
            .Select(DeductFundsFromBalance)
            .ToList();

        if (remainingAmount > 0)
        {
            throw new InsufficientFundsException(Id);
        }
        
        var transfer = new OutgoingTransfer(transferId, transactionId, Id, name, subTransfers, currency, amount, now, GetMetadata(transactionId, Id));
        _transfers.Add(transfer);
        
        IncrementVersion();

        return transfer;
        
        InternalTransfer DeductFundsFromBalance(Balance.Balance balance)
        {
            var exchangeRate = GetExchangeRate(balance.Currency, currency, exchangeRates);
            var exchangedAmountFromBalance = balance.Amount * exchangeRate.Rate;

            if (exchangedAmountFromBalance <= Amount.Zero)
            {
                return null;
            }
            
            if (exchangedAmountFromBalance >= remainingAmount)
            {
                var amountToDeduct = remainingAmount / exchangeRate.Rate;
                
                var internalTransfer = new OutgoingInternalTransfer(new TransferId(), transactionId, balance.Id, balance.Currency, amountToDeduct, now,
                    GetMetadata(transactionId, Id), exchangeRate);
                
                balance.AddTransfer(internalTransfer);
                remainingAmount = 0;
                return internalTransfer;
            }
            else
            {
                var amountToDeduct = balance.Amount;
                
                var internalTransfer = new OutgoingInternalTransfer(new TransferId(), transactionId, balance.Id, balance.Currency, amountToDeduct, now,
                    GetMetadata(transactionId, Id), exchangeRate);
                
                balance.AddTransfer(internalTransfer);
                remainingAmount -= exchangedAmountFromBalance;
                return internalTransfer;
            }
        }
    }

    public bool HasSufficientFunds(Amount amount, Currency currency, List<ExchangeRate> exchangeRates)
    {
        if (amount < Amount.Zero)
        {
            throw new NegativeAmountException();
        }
        
        var remainingAmount = amount;
        
        foreach (var balance in _balances
                     .OrderByDescending(x => x.IsPrimary)
                     .TakeWhile(_ => !(remainingAmount <= 0)))
            
        {
            DeductFundsFromBalance(balance);
        }

        return remainingAmount == Amount.Zero;

        void DeductFundsFromBalance(Balance.Balance balance)
        {
            var exchangeRate = GetExchangeRate(balance.Currency, currency, exchangeRates).Rate;
            var exchangedAmountFromBalance = balance.Amount * exchangeRate;

            if (exchangedAmountFromBalance <= Amount.Zero)
            {
                return;
            }
            
            if (exchangedAmountFromBalance >= remainingAmount)
            {
                remainingAmount = 0;
            }
            else
            {
                remainingAmount -= exchangedAmountFromBalance;
            }
        }
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
    
    private ExchangeRate GetExchangeRate(Currency from, Currency to, List<ExchangeRate> exchangeRates) => exchangeRates
        .Single(x => x.From == from && x.To == to);
    
    private static TransferMetadata GetMetadata(TransactionId transactionId, WalletId walletId)
        => new($"{{\"transactionId\": \"{transactionId}\", \"walletId\": \"{walletId}\"}}");
}