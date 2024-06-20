using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;

namespace Rapido.Services.Wallets.Application.Wallets.Commands.Handlers;

internal sealed class AddBalanceHandler : ICommandHandler<AddBalance>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IClock _clock;

    public AddBalanceHandler(IWalletRepository walletRepository, IClock clock)
    {
        _walletRepository = walletRepository;
        _clock = clock;
    }
    
    public async Task HandleAsync(AddBalance command)
    {
        var wallet = await _walletRepository.GetAsync(new OwnerId(command.OwnerId));

        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }

        var currency = new Currency(command.Currency);
        
        wallet.AddBalance(currency, _clock.Now());

        await _walletRepository.UpdateAsync(wallet);
    }
}