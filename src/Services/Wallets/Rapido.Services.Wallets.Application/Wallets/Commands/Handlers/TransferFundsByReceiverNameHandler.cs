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
using Rapido.Services.Wallets.Domain.Owners.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.DomainServices;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using OwnerNotFoundException = Rapido.Services.Wallets.Application.Wallets.Exceptions.OwnerNotFoundException;

namespace Rapido.Services.Wallets.Application.Wallets.Commands.Handlers;

internal sealed class TransferFundsByReceiverNameHandler(
    IWalletRepository walletRepository,
    IOwnerRepository ownerRepository,
    ITransferService transferService,
    IMessageBroker messageBroker,
    ITransactionIdGenerator transactionIdGenerator,
    ILogger<TransferFundsByWalletIdHandler> logger,
    ICurrencyApiClient client, 
    IClock clock)
    : ICommandHandler<TransferFundsByReceiverName>
{
    public async Task HandleAsync(TransferFundsByReceiverName command)
    {
        var ownerId = new OwnerId(command.OwnerId);
        var receiverName = new OwnerName(command.ReceiverName);
        var transferName = new TransferName(command.TransferName);
        var currency = new Currency(command.Currency);
        var amount = new Amount(command.Amount);
        
        var wallet = await walletRepository.GetAsync(ownerId);
        
        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }

        var receiver = await ownerRepository.GetAsync(receiverName);

        if (receiver is null)
        {
            throw new OwnerNotFoundException(receiverName);
        }

        var receiverWallet = await walletRepository.GetAsync(receiver.Id);
        
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
        
        await walletRepository.UpdateAsync(wallet);
        await walletRepository.UpdateAsync(receiverWallet);
        
        await messageBroker.PublishAsync(
            new IEvent[]
            {
                new FundsDeducted(wallet.Id, wallet.OwnerId,transactionId, transferName, currency, amount, now),
                new FundsAdded(wallet.Id, wallet.OwnerId,transactionId, transferName, currency, amount, now)
            });
        
        logger.LogInformation("Transferred {amount} {currency} from wallet with id: {walletId} to wallet with id: {receiverWalletId}.", amount, currency, wallet.Id, receiverWallet.Id);
    }
}