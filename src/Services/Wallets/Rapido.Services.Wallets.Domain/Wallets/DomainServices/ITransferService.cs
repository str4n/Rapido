using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Domain.Wallets.DomainServices;

public interface ITransferService
{
    void Transfer(Wallet.Wallet wallet, Wallet.Wallet receiverWallet, TransferName transferName, Amount amount,
        Currency currency, List<ExchangeRate> exchangeRates);
}