﻿using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public sealed class IncomingTransfer : Transfer
{
    public IncomingTransfer(TransferId id, WalletId walletId, TransferName name,
        Currency currency, Amount amount, DateTime createdAt, TransferMetadata metadata = null) 
        : base(id, walletId, name, currency, amount, createdAt, metadata)
    {
    }

    private IncomingTransfer()
    {
    }
}