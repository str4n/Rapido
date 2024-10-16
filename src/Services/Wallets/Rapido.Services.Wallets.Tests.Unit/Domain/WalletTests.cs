using FluentAssertions;
using Rapido.Services.Wallets.Domain.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Tests.Unit.Domain;

public class WalletTests
{
    [Fact]
    public void add_funds_in_primary_currency_should_succeed()
    {
        //Arrange
        
        var primaryCurrency = new Currency("PLN");
        var wallet = Wallet.Create(Guid.NewGuid(), primaryCurrency, _now);
        var amount = 20.0;
        var exchangeRate = TestExchangeRates.GetExchangeRate(primaryCurrency, primaryCurrency);
        
        //Act

        var transfer = wallet.AddFunds("name", amount, primaryCurrency, exchangeRate, _now);
        
        //Assert

        var primaryBalanceAmount = wallet.Balances.Single(x => x.IsPrimary).Amount.Value;
        
        primaryBalanceAmount.Should().Be(amount);
        transfer.Should().BeOfType<IncomingTransfer>();
        transfer.Amount.Value.Should().Be(amount);
        transfer.Currency.Value.Should().Be(primaryCurrency);
        wallet.Transfers.Should().ContainEquivalentOf(transfer);
    }
    
    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("GBP")]
    public void add_funds_in_non_primary_currency_should_succeed(string currency)
    {
        //Arrange
        
        var primaryCurrency = new Currency("PLN");
        var wallet = Wallet.Create(Guid.NewGuid(), primaryCurrency, _now);
        var amount = 20.0;
        var exchangeRate = TestExchangeRates.GetExchangeRate(currency, primaryCurrency);
        var exchangedAmount = amount * exchangeRate.Rate;
        
        //Act

        var transfer = wallet.AddFunds("name", amount, currency, exchangeRate, _now);
        
        //Assert

        var primaryBalanceAmount = wallet.Balances.Single(x => x.IsPrimary).Amount.Value;
        
        primaryBalanceAmount.Should().Be(exchangedAmount);
        transfer.Should().BeOfType<IncomingTransfer>();
        transfer.Amount.Value.Should().Be(amount);
        transfer.Currency.Value.Should().Be(currency);
        wallet.Transfers.Should().ContainEquivalentOf(transfer);
    }

    [Fact]
    public void deduct_funds_from_wallet_with_single_balance_should_succeed()
    {
        //Arrange
        
        var primaryCurrency = new Currency("PLN");
        var wallet = Wallet.Create(Guid.NewGuid(), primaryCurrency, _now);
        var amount = 20.0;
        wallet.Balances.Single(x => x.IsPrimary).AddFunds(amount);
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        
        //Act

        var transfer = wallet.DeductFunds("name", amount, primaryCurrency, exchangeRates , _now);
        
        //Assert

        var primaryBalanceAmount = wallet.Balances.Single(x => x.IsPrimary).Amount.Value;

        primaryBalanceAmount.Should().Be(Amount.Zero);
        transfer.Should().BeOfType<OutgoingTransfer>();
        transfer.Amount.Value.Should().Be(amount);
        transfer.Currency.Value.Should().Be(primaryCurrency);
        wallet.Transfers.Should().ContainEquivalentOf(transfer);
    }
    
    [Fact]
    public void deduct_funds_from_wallet_with_single_balance_should_throw_insufficient_funds_exception()
    {
        //Arrange
        
        var primaryCurrency = new Currency("PLN");
        var wallet = Wallet.Create(Guid.NewGuid(), primaryCurrency, _now);
        var amount = 20.0;
        wallet.Balances.Single(x => x.IsPrimary).AddFunds(amount);
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        
        //Act

        var act = () => wallet.DeductFunds("name", 30, primaryCurrency, exchangeRates , _now);
        
        //Assert

        act.Should().Throw<InsufficientFundsException>();
    }

    [Fact]
    public void deduct_funds_from_wallet_with_many_balances_should_deduct_funds_firstly_form_primary()
    {
        //Arrange
        
        var primaryCurrency = new Currency("PLN");
        var eurCurrency = new Currency("EUR");
        var wallet = Wallet.Create(Guid.NewGuid(), primaryCurrency, _now);
        wallet.AddBalance(eurCurrency, _now);
        var amount = 20.0;
        var eurAmount = 6.0;
        wallet.Balances.Single(x => x.IsPrimary).AddFunds(amount);
        wallet.Balances.Single(x => x.Currency == eurCurrency).AddFunds(eurAmount);
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        
        //Act

        var transfer = wallet.DeductFunds("name", amount, primaryCurrency, exchangeRates , _now);
        
        //Assert

        var primaryBalanceAmount = wallet.Balances.Single(x => x.IsPrimary).Amount.Value;
        var eurBalanceAmount = wallet.Balances.Single(x => x.Currency == eurCurrency).Amount.Value;

        primaryBalanceAmount.Should().Be(Amount.Zero);
        eurBalanceAmount.Should().Be(eurAmount);
        transfer.Should().BeOfType<OutgoingTransfer>();
        transfer.Amount.Value.Should().Be(amount);
        transfer.Currency.Value.Should().Be(primaryCurrency);
        wallet.Transfers.Should().ContainEquivalentOf(transfer);
    }

    #region Arrange
    
    private readonly DateTime _now;

    public WalletTests()
    {
        _now = new DateTime(2024, 10, 16);
    }

    #endregion
}