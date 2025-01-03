﻿using Rapido.Services.Wallets.Domain.Wallets.Balance;
using Rapido.Services.Wallets.Domain.Wallets.Money;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public sealed class IncomingInternalTransfer : InternalTransfer
{
    public IncomingInternalTransfer(TransferId id, TransactionId transactionId, BalanceId balanceId, Currency currency, 
        Amount amount, DateTime createdAt, TransferMetadata metadata = null, ExchangeRate exchangeRate = null) 
        : base(id, transactionId, balanceId, currency, amount, createdAt, metadata, exchangeRate)
    {
    }

    private IncomingInternalTransfer()
    {
    }
}