using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public sealed class OutgoingTransfer : Transfer
{
    public OutgoingTransfer(TransferId id, WalletId walletId, TransferName name, TransferDescription description, 
        Currency currency, Amount amount, DateTime createdAt, TransferMetadata metadata = null) 
        : base(id, walletId, name, description, currency, amount, createdAt, metadata)
    {
    }
}