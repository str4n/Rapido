﻿using Microsoft.Extensions.Logging;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Events;
using Rapido.Services.Wallets.Application.Wallets.Clients;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.DomainServices;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Application.Wallets.Commands.Handlers;

internal sealed class TransferFundsByWalletIdHandler : ICommandHandler<TransferFundsByWalletId>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITransferService _transferService;
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<TransferFundsByWalletIdHandler> _logger;
    private readonly ICurrencyApiClient _client;

    public TransferFundsByWalletIdHandler(IWalletRepository walletRepository, ITransferService transferService,
        IMessageBroker messageBroker, ILogger<TransferFundsByWalletIdHandler> logger, ICurrencyApiClient client)
    {
        _walletRepository = walletRepository;
        _transferService = transferService;
        _messageBroker = messageBroker;
        _logger = logger;
        _client = client;
    }
    
    public async Task HandleAsync(TransferFundsByWalletId command)
    {
        var walletId = new WalletId(command.WalletId);
        var receiverWalletId = new WalletId(command.ReceiverWalletId);
        var transferName = new TransferName(command.TransferName);
        var currency = new Currency(command.Currency);
        var amount = new Amount(command.Amount);
        
        var wallet = await _walletRepository.GetAsync(walletId);
        
        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }
        
        if (wallet.OwnerId != (OwnerId)command.OwnerId)
        {
            throw new WalletNotFoundException();
        }
        
        var receiverWallet = await _walletRepository.GetAsync(receiverWalletId);
        
        if (receiverWallet is null)
        {
            throw new WalletNotFoundException();
        }

        if (wallet.Id == receiverWallet.Id)
        {
            throw new CannotMakeSelfFundsTransferException();
        }

        var exchangeRates = (await _client.GetExchangeRates()).ToList();
        
        if (exchangeRates is null || !exchangeRates.Any())
        {
            throw new ExchangeRateNotFoundException();
        }
        
        _transferService.Transfer(wallet, receiverWallet, transferName, amount, currency, exchangeRates);
        
        await _walletRepository.UpdateAsync(wallet);
        await _walletRepository.UpdateAsync(receiverWallet);
        
        await _messageBroker.PublishAsync(
            new FundsTransferred(wallet.Id, receiverWallet.Id, transferName, currency, amount));
        
        _logger.LogInformation("Transferred {amount} {currency} from wallet with id: {walletId} to wallet with id: {receiverWalletId}.", amount, currency, walletId, receiverWalletId);
    }
}