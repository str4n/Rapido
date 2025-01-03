using FluentAssertions;
using Rapido.Services.Wallets.Domain.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Tests.Unit.Domain;

public class WalletHasSufficientFundsTests
{
    [Theory]
    [InlineData(2)]
    [InlineData(30)]
    [InlineData(0)]
    public void given_wallet_with_sufficient_funds__method_has_sufficient_funds_should_return_true(double amount)
    {
        var currency = new Currency("EUR");
        var wallet = Wallet.Create(Guid.NewGuid(), currency, _now);
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        wallet.AddFunds("name", 30, currency, exchangeRates, _now);

        var result = wallet.HasSufficientFunds(amount, currency, exchangeRates);

        result.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(1.5)]
    [InlineData(30)]
    [InlineData(999)]
    public void given_wallet_with_insufficient_funds__method_has_sufficient_funds_should_return_false(double amount)
    {
        var currency = new Currency("EUR");
        var wallet = Wallet.Create(Guid.NewGuid(), currency, _now);
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        wallet.AddFunds("name", 1, currency, exchangeRates, _now);

        var result = wallet.HasSufficientFunds(amount, currency, exchangeRates);

        result.Should().BeFalse();
    }

    [Fact]
    public void given_negative_funds_amount_to_has_sufficient_funds_method_should_throw_exception()
    {
        var amount = -1;
        var currency = new Currency("EUR");
        var wallet = Wallet.Create(Guid.NewGuid(), currency, _now);
        var exchangeRates = TestExchangeRates.GetExchangeRates();
        wallet.AddFunds("name", 30, currency, exchangeRates, _now);

        var act = () => wallet.HasSufficientFunds(amount, currency, exchangeRates);

        act.Should().Throw<NegativeAmountException>();
    }
    
    #region Arrange
    
    private readonly DateTime _now;

    public WalletHasSufficientFundsTests()
    {
        _now = new DateTime(2024, 10, 16);
    }

    #endregion
}