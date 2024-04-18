using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Aggregate;
using Rapido.Services.Wallets.Domain.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Domain.Wallets.Wallet;

public sealed class Wallet : AggregateRoot
{
    public OwnerId OwnerId { get; }
    public Amount Amount => CurrentAmount();
    public Currency Currency { get; }

    private HashSet<Transfer.Transfer> _transfers = new();
    public IEnumerable<Transfer.Transfer> Transfers => _transfers;
    public DateTime CreatedAt { get; }

    public Wallet(WalletId id, OwnerId ownerId, Currency currency, DateTime createdAt)
    {
        Id = new(id);
        OwnerId = ownerId;
        Currency = currency;
        CreatedAt = createdAt;
    }

    public void TransferFunds(Wallet receiverWallet, TransferName name, TransferDescription description,
        Amount amount, DateTime now)
    {
        var outgoingTransferId = new TransferId();
        var incomingTransferId = new TransferId();
        
        var outgoingTransfer =
            new OutgoingTransfer(outgoingTransferId, new WalletId(Id), name, description, Currency, 
                amount, now, GetMetadata(outgoingTransferId, new WalletId(receiverWallet.Id)));
        
        var incomingTransfer =
            new IncomingTransfer(incomingTransferId, new WalletId(receiverWallet.Id), name, description, Currency, 
                amount, now, GetMetadata(incomingTransferId, new WalletId(Id)));
        
        TransferFunds(outgoingTransfer);
        receiverWallet.ReceiveFunds(incomingTransfer);
        
        static TransferMetadata GetMetadata(TransferId transferId, WalletId walletId)
            => new($"{{\"transferId\": \"{transferId}\", \"walletId\": \"{walletId}\"}}");
    }

    private void ReceiveFunds(IncomingTransfer transfer)
    {
        if (transfer.Amount <= 0)
        {
            throw new InvalidTransferAmountException(transfer.Amount);
        }
        
        _transfers.Add(transfer);
        IncrementVersion();
    }
    
    private void TransferFunds(OutgoingTransfer transfer)
    {
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
    }
    

    private Amount CurrentAmount() => _transfers.OfType<IncomingTransfer>().Sum(x => x.Amount) -
                                     _transfers.OfType<OutgoingTransfer>().Sum(x => x.Amount);
}