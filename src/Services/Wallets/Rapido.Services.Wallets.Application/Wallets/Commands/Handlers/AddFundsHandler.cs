using Microsoft.Extensions.Logging;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Wallets.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Application.Wallets.Commands.Handlers;

internal sealed class AddFundsHandler : ICommandHandler<AddFunds>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<AddFundsHandler> _logger;

    public AddFundsHandler(IWalletRepository walletRepository, IClock clock, 
        IMessageBroker messageBroker, ILogger<AddFundsHandler> logger)
    {
        _walletRepository = walletRepository;
        _clock = clock;
        _messageBroker = messageBroker;
        _logger = logger;
    }
    
    public async Task HandleAsync(AddFunds command)
    {
        var walletId = new WalletId(command.WalletId);
        var currency = new Currency(command.Currency);
        var amount = new Amount(command.Amount);
        var transferName = new TransferName(command.TransferName);
        var transferDescription = new TransferDescription(command.TransferDescription);

        var wallet = await _walletRepository.GetAsync(walletId);
        
        if (wallet is null)
        {
            throw new WalletNotFoundException(walletId);
        }

        if (wallet.Currency != currency)
        {
            throw new TransferCurrencyMismatchException($"Receiver wallet supports only: {wallet.Currency} currency.");
        }
        
        var now = _clock.Now();

        var transfer = wallet.AddFunds(wallet, transferName, transferDescription, amount, now);

        await _walletRepository.UpdateAsync(wallet);

        await _messageBroker.PublishAsync(new FundsAdded(transfer.WalletId, transfer.Name, transfer.Currency, transfer.Amount));
        
        _logger.LogInformation("{amount} {currency} added to wallet with id: {walletId}.", amount, currency, walletId);
    }
}