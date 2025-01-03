using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public sealed class OutgoingTransfer : Transfer
{
    public OutgoingTransfer(TransferId id, TransactionId transactionId, WalletId walletId, TransferName name, IEnumerable<InternalTransfer> subTransfers,
        Currency currency, Amount amount, DateTime createdAt, TransferMetadata metadata = null) 
        : base(id, transactionId, walletId, name, subTransfers, currency, amount, createdAt, metadata)
    {
    }

    private OutgoingTransfer()
    {
    }
}