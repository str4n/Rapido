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
    public Wallet(WalletId id, OwnerId ownerId, Currency currency, DateTime createdAt)
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

    public Transaction TransferFunds(Wallet receiverWallet, TransferName name, Amount amount, Currency currency, 
        List<ExchangeRate> exchangeRates, DateTime now)
    {
        var incomingTransfer = receiverWallet.AddFunds(name, amount, 
            currency, exchangeRates.SingleOrDefault(x => x.From == currency && x.To == receiverWallet.GetPrimaryCurrency()), now);

        var outgoingTransfer = DeductFunds(name, amount, currency, exchangeRates, now);

        return new Transaction(outgoingTransfer, incomingTransfer);
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
        
        if (amount <= 0)
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

        var primaryBalance = GetPrimaryBalance();
        var primaryCurrency = GetPrimaryCurrency();

        if (primaryBalance is null)
        {
            throw new BalanceNotFoundException();
        }
        
        if (amount <= 0)
        {
            throw new InvalidTransferAmountException(amount);
        }
        
        var exchangeRatePrimary = GetExchangeRate(primaryCurrency, currency);

        var exchangedAmount = primaryBalance.Amount * exchangeRatePrimary.Value;

        var transfer = new OutgoingTransfer(transferId, Id, name, currency, amount, now, GetMetadata(transferId, Id));
        
        var remainingAmount = amount.Value;
        
        if (exchangedAmount >= amount)
        {
            var amountToDeductFromPrimary = remainingAmount / exchangeRatePrimary.Value;
            primaryBalance.DeductFunds(amountToDeductFromPrimary);
            remainingAmount = 0;
        }
        else
        {
            var amountToDeductFromPrimary = primaryBalance.Amount;
            primaryBalance.DeductFunds(amountToDeductFromPrimary);
            remainingAmount -= exchangedAmount;
        }
        
        foreach (var balance in _balances.Where(b => b.Currency != primaryCurrency))
        {
            if (remainingAmount <= 0)
            {
                break;
            }

            var exchangeRate = GetExchangeRate(balance.Currency, currency);

            var balanceInTargetCurrency = balance.Amount * exchangeRate.Value;

            if (balanceInTargetCurrency >= remainingAmount)
            {
                var amountToDeduct = remainingAmount / exchangeRate.Value;
                balance.DeductFunds(amountToDeduct);
                remainingAmount = 0;
            }
            else
            {
                var amountToDeduct = balance.Amount;
                balance.DeductFunds(amountToDeduct);
                remainingAmount -= balanceInTargetCurrency;
            }
        }

        if (remainingAmount > 0)
        {
            throw new InsufficientWalletFundsException(Id);
        }
        
        _transfers.Add(transfer);
        
        IncrementVersion();

        return transfer;
        
        double? GetExchangeRate(Currency from, Currency to) => exchangeRates
            .SingleOrDefault(x => x.From == from && x.To == to)?.Rate;
    }
    

    public bool BalanceExists(Currency currency) => _balances.Any(x => x.Currency == currency);
    public Currency GetPrimaryCurrency() => _balances.SingleOrDefault(x => x.IsPrimary)?.Currency;

    public Amount GetAmount(Currency currency)
    {
        var balance = GetBalance(currency);

        if (balance is null)
        {
            throw new BalanceNotFoundException();
        }

        return balance.Amount;
    }

    private Balance.Balance GetBalance(Currency currency) 
        => _balances.SingleOrDefault(x => x.Currency == currency);

    private Balance.Balance GetPrimaryBalance() 
        => _balances.SingleOrDefault(x => x.IsPrimary);
    
    private static TransferMetadata GetMetadata(TransferId transferId, WalletId walletId)
        => new($"{{\"transferId\": \"{transferId}\", \"walletId\": \"{walletId}\"}}");
}