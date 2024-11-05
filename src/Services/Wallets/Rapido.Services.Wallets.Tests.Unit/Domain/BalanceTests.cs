using FluentAssertions;
using Rapido.Services.Wallets.Domain.Wallets.Money;

namespace Rapido.Services.Wallets.Tests.Unit.Domain;

public class BalanceTests
{
    [Fact]
    public void add_funds_to_balance_should_succeed()
    {
        var balance = Wallets.Domain.Wallets.Balance.Balance.Create(Guid.NewGuid(), new Currency("EUR"), true, _now);
        var amount = new Amount(20);
        
        balance.AddFunds(amount);

        balance.Amount.Should().Be(amount);
    }
    
    [Fact]
    public void deduct_funds_from_balance_should_succeed()
    {
        var balance = Wallets.Domain.Wallets.Balance.Balance.Create(Guid.NewGuid(), new Currency("EUR"), true, _now);
        var amount = new Amount(20);
        balance.AddFunds(amount);
        
        balance.DeductFunds(amount);

        balance.Amount.Should().Be(Amount.Zero);
    }

    #region ARRANGE

    private readonly DateTime _now;

    public BalanceTests()
    {
        _now = new DateTime(2024, 10, 16);
    }

    #endregion
}