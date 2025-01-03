using Rapido.Framework.Common.Time;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Domain.Wallets.DomainServices;

internal sealed class TransferService(IClock clock) : ITransferService
{
    public void Transfer(Wallet.Wallet wallet, Wallet.Wallet receiverWallet, TransferName transferName, Amount amount, Currency currency,
        List<ExchangeRate> exchangeRates)
    {
        var now = clock.Now();

        wallet.DeductFunds(transferName, amount, currency, exchangeRates, now);
        receiverWallet.AddFunds(transferName, amount, currency, exchangeRates, now);
    }
}