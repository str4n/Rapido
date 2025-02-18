﻿using FluentAssertions;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Wallets.Domain.Wallets.DomainServices;
using Rapido.Services.Wallets.Domain.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Tests.Unit.Domain;

public class TransferDomainServiceTests
{
    [Fact]
    public void given_wallet_with_sufficient_funds__transfer_should_succeed()
    {
        //Arrange
        
        var eurCurrency = new Currency("EUR");

        var senderWallet = Wallet.Create(Guid.NewGuid(), eurCurrency, _clock.Now());
        var receiverWallet = Wallet.Create(Guid.NewGuid(), eurCurrency, _clock.Now());
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        var amount = new Amount(10);
        
        senderWallet.AddFunds(TransactionId.Create(),"name", amount, Currency.EUR(), exchangeRates, _clock.Now());
        
        //Act
        
        _service.Transfer(senderWallet, receiverWallet, TransactionId.Create(), "name", amount, eurCurrency, exchangeRates, _clock.Now());
        
        //Assert

        senderWallet.GetTotalFunds(exchangeRates).Value.Should().Be(Amount.Zero);
        senderWallet.Transfers.Should().HaveCount(2);
        
        var outgoingTransfer = senderWallet.Transfers.Last();
        outgoingTransfer.Should().BeOfType<OutgoingTransfer>();
        outgoingTransfer.Amount.Should().Be(amount);
        outgoingTransfer.Currency.Should().Be(eurCurrency);

        receiverWallet.GetTotalFunds(exchangeRates).Value.Should().Be(amount);
        receiverWallet.Transfers.Should().ContainSingle();
        
        var incomingTransfer = receiverWallet.Transfers.First();
        incomingTransfer.Should().BeOfType<IncomingTransfer>();
        incomingTransfer.Amount.Should().Be(amount);
        incomingTransfer.Currency.Should().Be(eurCurrency);
    }
    
    [Fact]
    public void given_wallet_with_insufficient_funds__transfer_should_throw_exception()
    {
        //Arrange
        
        var eurCurrency = new Currency("EUR");

        var senderWallet = Wallet.Create(Guid.NewGuid(), eurCurrency, _clock.Now());
        var receiverWallet = Wallet.Create(Guid.NewGuid(), eurCurrency, _clock.Now());
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        var amount = new Amount(10);
        var transferAmount = new Amount(20);
        
        senderWallet.AddFunds(TransactionId.Create(),"name", amount, Currency.EUR(), exchangeRates, _clock.Now());
        
        //Act
        
        var act = 
            () => _service.Transfer(senderWallet, receiverWallet, TransactionId.Create(),"name", transferAmount, eurCurrency, exchangeRates, _clock.Now());
        
        //Assert

        act.Should().Throw<InsufficientFundsException>();
    }
    
    
    #region Arrange

    private readonly ITransferService _service = new TransferService();
    private readonly IClock _clock = new TestClock();

    #endregion
}