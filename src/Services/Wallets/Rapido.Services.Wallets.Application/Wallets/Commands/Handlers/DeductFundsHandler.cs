using Microsoft.Extensions.Logging;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Events;
using Rapido.Services.Wallets.Application.Wallets.Clients;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Application.Wallets.Services;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Application.Wallets.Commands.Handlers;

internal sealed class DeductFundsHandler(
    IWalletRepository walletRepository,
    IClock clock,
    ICurrencyApiClient client,
    IMessageBroker messageBroker,
    ITransactionIdGenerator generator,
    ILogger<DeductFundsHandler> logger)
    : ICommandHandler<DeductFunds>
{
    public async Task HandleAsync(DeductFunds command, CancellationToken cancellationToken = default)
    {
        var walletId = new WalletId(command.WalletId);
        var currency = new Currency(command.Currency);
        var amount = new Amount(command.Amount);
        var transferName = new TransferName(command.TransferName);
        
        var wallet = await walletRepository.GetAsync(walletId, cancellationToken);
        
        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }
        
        var now = clock.Now();

        var exchangeRates = (await client.GetExchangeRates()).ToList();
        
        if (exchangeRates is null || !exchangeRates.Any())
        {
            throw new ExchangeRateNotFoundException();
        }

        var transactionId = await generator.Generate();
        
        wallet.DeductFunds(transactionId, transferName, amount, currency, exchangeRates.ToList(), now);
        
        await walletRepository.UpdateAsync(wallet, cancellationToken);
        
        await messageBroker.PublishAsync(new FundsDeducted(
            walletId, 
            wallet.OwnerId, 
            transactionId,
            transferName, 
            currency, 
            amount, 
            now));
        
        logger.LogInformation("{amount} {currency} deducted from wallet with id: {walletId}.", amount, currency, walletId);
    }
}