using Rapido.Framework.Common.Time;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Domain.Wallets.DomainServices;

internal sealed class TransferService : ITransferService
{
    private readonly IClock _clock;

    public TransferService(IClock clock)
    {
        _clock = clock;
    }
    
    public void Transfer(Wallet.Wallet wallet, Wallet.Wallet receiverWallet, TransferName transferName, Amount amount, Currency currency,
        List<ExchangeRate> exchangeRates)
    {
        var exchangeRate =
            exchangeRates.SingleOrDefault(x => x.From == currency && x.To == receiverWallet.GetPrimaryCurrency());

        var now = _clock.Now();

        wallet.DeductFunds(transferName, amount, currency, exchangeRates, now);
        receiverWallet.AddFunds(transferName, amount, currency, exchangeRate, now);
    }
}