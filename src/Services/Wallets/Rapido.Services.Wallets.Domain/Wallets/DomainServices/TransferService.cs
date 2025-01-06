using Rapido.Framework.Common.Time;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Domain.Wallets.DomainServices;

internal sealed class TransferService : ITransferService
{
    public void Transfer(Wallet.Wallet wallet, Wallet.Wallet receiverWallet, TransactionId transactionId, TransferName transferName, Amount amount, Currency currency,
        List<ExchangeRate> exchangeRates, DateTime date)
    {
        wallet.DeductFunds(transactionId, transferName, amount, currency, exchangeRates, date);
        receiverWallet.AddFunds(transactionId, transferName, amount, currency, exchangeRates, date);
    }
}