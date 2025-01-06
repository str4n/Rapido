using FluentAssertions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Tests.Unit.Domain;

public class WalletAddFundsTests
{
    [Fact]
    public void add_funds_in_primary_currency_should_succeed()
    {
        //Arrange
        
        var primaryCurrency = new Currency("PLN");
        var wallet = Wallet.Create(Guid.NewGuid(), primaryCurrency, _now);
        var amount = 20.0;
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        
        //Act

        var transfer = wallet.AddFunds(TransactionId.Create(),"name", amount, primaryCurrency, exchangeRates, _now);
        
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
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        var exchangeRate = exchangeRates.Single(x => x.From == currency && x.To == primaryCurrency);
        var exchangedAmount = amount * exchangeRate.Rate;
        
        //Act

        var transfer = wallet.AddFunds(TransactionId.Create(),"name", amount, currency, exchangeRates, _now);
        
        //Assert

        var primaryBalanceAmount = wallet.Balances.Single(x => x.IsPrimary).Amount.Value;
        
        primaryBalanceAmount.Should().Be(exchangedAmount);
        transfer.Should().BeOfType<IncomingTransfer>();
        transfer.Amount.Value.Should().Be(amount);
        transfer.Currency.Value.Should().Be(currency);
        wallet.Transfers.Should().ContainEquivalentOf(transfer);
    }

    

    #region Arrange
    
    private readonly DateTime _now;

    public WalletAddFundsTests()
    {
        _now = new DateTime(2024, 10, 16);
    }

    #endregion
}