using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;

namespace Rapido.Services.Wallets.Application.Wallets.Commands.Handlers;

internal sealed class AddBalanceHandler(IWalletRepository walletRepository, IClock clock) : ICommandHandler<AddBalance>
{
    public async Task HandleAsync(AddBalance command, CancellationToken cancellationToken = default)
    {
        var wallet = await walletRepository.GetAsync(new OwnerId(command.OwnerId), cancellationToken);

        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }

        var currency = new Currency(command.Currency);
        
        wallet.AddBalance(currency, clock.Now());

        await walletRepository.UpdateAsync(wallet, cancellationToken);
    }
}