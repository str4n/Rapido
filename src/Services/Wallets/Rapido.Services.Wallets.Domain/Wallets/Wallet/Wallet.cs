using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Aggregate;
using Rapido.Services.Wallets.Domain.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Domain.Wallets.Wallet;

public sealed class Wallet : AggregateRoot<WalletId>
{
    public OwnerId OwnerId { get; }
    public Amount Amount => CurrentAmount();
    public Currency Currency { get; }

    private readonly HashSet<Transfer.Transfer> _transfers = new();
    public IEnumerable<Transfer.Transfer> Transfers => _transfers;
    public DateTime CreatedAt { get; }
    public Wallet(WalletId id, OwnerId ownerId, Currency currency, DateTime createdAt)
    {
        Id = new(id);
        OwnerId = ownerId;
        Currency = currency;
        CreatedAt = createdAt;
    }

    private Wallet()
    {
    }

    public static Wallet Create(OwnerId ownerId, Currency currency, DateTime createdAt)
        => new Wallet(Guid.NewGuid(), ownerId, currency, createdAt);

    public Transaction TransferFunds(Wallet receiverWallet, TransferName name, TransferDescription description,
        Amount amount, DateTime now)
    {
        var incomingTransfer = receiverWallet.AddFunds(receiverWallet, name, description, amount, now);
        var outgoingTransfer = DeductFunds(receiverWallet, name, description, amount, now);

        return new Transaction(outgoingTransfer, incomingTransfer);
    }

    public IncomingTransfer AddFunds(Wallet receiverWallet, TransferName name, TransferDescription description, 
        Amount amount, DateTime now)
    {
        var transferId = new TransferId();
        
        var transfer =
            new IncomingTransfer(transferId, receiverWallet.Id, name, description, Currency, 
                amount, now, GetMetadata(transferId, Id));
        
        if (transfer.Amount <= 0)
        {
            throw new InvalidTransferAmountException(transfer.Amount);
        }
        
        _transfers.Add(transfer);
        IncrementVersion();
        
        return transfer;
    }
    
    public OutgoingTransfer DeductFunds(Wallet receiverWallet, TransferName name, TransferDescription description, 
        Amount amount, DateTime now)
    {
        var transferId = new TransferId();
        
        var transfer =
            new OutgoingTransfer(transferId, Id, name, description, Currency, 
                amount, now, GetMetadata(transferId, receiverWallet.Id));
        
        if (transfer.Amount <= 0)
        {
            throw new InvalidTransferAmountException(transfer.Amount);
        }

        if (transfer.Amount > Amount)
        {
            throw new InsufficientWalletFundsException(Id);
        }
        
        _transfers.Add(transfer);
        IncrementVersion();

        return transfer;
    }
    
    private static TransferMetadata GetMetadata(TransferId transferId, WalletId walletId)
        => new($"{{\"transferId\": \"{transferId}\", \"walletId\": \"{walletId}\"}}");
    

    private Amount CurrentAmount() => _transfers.OfType<IncomingTransfer>().Sum(x => x.Amount) -
                                     _transfers.OfType<OutgoingTransfer>().Sum(x => x.Amount);
}