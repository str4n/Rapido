using FluentAssertions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Tests.Unit.Domain;

public class WalletTests
{
    [Fact]
    public void given_wallet_with_two_balances_get_total_amount_method_should_return_proper_value()
    {
        //Arrange
        
        var eurCurrency = new Currency("EUR"); //Primary currency
        var usdCurrency = new Currency("USD"); //Secondary currency
        var wallet = Wallet.Create(new Guid(), eurCurrency, _now);
        wallet.AddBalance(usdCurrency, _now);

        var eurBalance = wallet.Balances.Single(x => x.Currency == eurCurrency);
        var usdBalance = wallet.Balances.Single(x => x.Currency == usdCurrency);

        var eurAmount = new Amount(10);
        var usdAmount = new Amount(10);
        
        eurBalance.AddFunds(eurAmount);
        usdBalance.AddFunds(usdAmount);
        
        //Act

        var result = wallet.GetTotalFunds(TestExchangeRates.GetExchangeRates());
        
        //Assert

        var exchangeRate = TestExchangeRates.GetExchangeRate(usdCurrency, eurCurrency).Rate;
        var expectedResult = eurAmount + usdAmount * exchangeRate;

        result.Value.Should().Be(expectedResult);
    }
    
    #region Arrange
    
    private readonly DateTime _now;

    public WalletTests()
    {
        _now = new DateTime(2024, 10, 16);
    }

    #endregion
}