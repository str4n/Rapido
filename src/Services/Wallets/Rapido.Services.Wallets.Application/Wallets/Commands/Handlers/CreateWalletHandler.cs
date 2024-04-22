using Microsoft.Extensions.Logging;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Wallets.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Application.Wallets.Commands.Handlers;

internal sealed class CreateWalletHandler : ICommandHandler<CreateWallet>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public CreateWalletHandler(IWalletRepository walletRepository, IClock clock, IMessageBroker messageBroker)
    {
        _walletRepository = walletRepository;
        _clock = clock;
        _messageBroker = messageBroker;
    }
    
    public async Task HandleAsync(CreateWallet command)
    {
        var ownerId = new OwnerId(command.OwnerId);
        var currency = new Currency(command.Currency);

        var existingWallet = await _walletRepository.GetAsync(ownerId, currency);

        if (existingWallet is not null)
        {
            throw new WalletAlreadyExistsException(ownerId, currency);
        }

        var wallet = Wallet.Create(ownerId, currency, _clock.Now());

        await _walletRepository.AddAsync(wallet);
        await _messageBroker.PublishAsync(new WalletCreated(wallet.Id, ownerId, currency));
    }
}