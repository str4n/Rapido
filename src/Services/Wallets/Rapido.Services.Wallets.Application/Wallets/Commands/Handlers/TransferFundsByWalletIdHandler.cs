using Microsoft.Extensions.Logging;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Abstractions.Events;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Events;
using Rapido.Services.Wallets.Application.Wallets.Clients;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Application.Wallets.Services;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.DomainServices;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Application.Wallets.Commands.Handlers;

internal sealed class TransferFundsByWalletIdHandler(
    IWalletRepository walletRepository,
    ITransferService transferService,
    IMessageBroker messageBroker,
    ITransactionIdGenerator transactionIdGenerator,
    ILogger<TransferFundsByWalletIdHandler> logger,
    ICurrencyApiClient client,
    IClock clock)
    : ICommandHandler<TransferFundsByWalletId>
{
    public async Task HandleAsync(TransferFundsByWalletId command, CancellationToken cancellationToken = default)
    {
        var walletId = new WalletId(command.WalletId);
        var receiverWalletId = new WalletId(command.ReceiverWalletId);
        var transferName = new TransferName(command.TransferName);
        var currency = new Currency(command.Currency);
        var amount = new Amount(command.Amount);
        
        var wallet = await walletRepository.GetAsync(walletId, cancellationToken);
        
        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }
        
        if (wallet.OwnerId != (OwnerId)command.OwnerId)
        {
            throw new WalletNotFoundException();
        }
        
        var receiverWallet = await walletRepository.GetAsync(receiverWalletId, cancellationToken);
        
        if (receiverWallet is null)
        {
            throw new WalletNotFoundException();
        }

        var now = clock.Now();

        if (wallet.Id == receiverWallet.Id)
        {
            throw new CannotMakeSelfFundsTransferException();
        }

        var exchangeRates = (await client.GetExchangeRates()).ToList();
        
        if (exchangeRates is null || !exchangeRates.Any())
        {
            throw new ExchangeRateNotFoundException();
        }

        var transactionId = await transactionIdGenerator.Generate();
        
        transferService.Transfer(wallet, receiverWallet, transactionId, transferName, amount, currency, exchangeRates, now);
        
        await walletRepository.UpdateAsync(wallet, cancellationToken);
        await walletRepository.UpdateAsync(receiverWallet, cancellationToken);
        
        await messageBroker.PublishAsync(
            new IEvent[]
            {
                new FundsDeducted(wallet.Id, wallet.OwnerId, transactionId, transferName, currency, amount, now),
                new FundsAdded(wallet.Id, wallet.OwnerId, transactionId, transferName, currency, amount, now),
            });
        
        logger.LogInformation("Transferred {amount} {currency} from wallet with id: {walletId} to wallet with id: {receiverWalletId}.", amount, currency, walletId, receiverWalletId);
    }
}