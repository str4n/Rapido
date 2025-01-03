using FluentAssertions;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;

namespace Rapido.Services.Wallets.Tests.Unit.Domain;

public class BalanceTests
{
    [Fact]
    public void add_funds_to_balance_should_succeed()
    {
        var currency = new Currency("EUR");
        var balance = Wallets.Domain.Wallets.Balance.Balance.Create(Guid.NewGuid(), currency, true, _now);
        var amount = new Amount(20);

        var subTransfer = new IncomingInternalTransfer(new TransferId(), new TransactionId(), Guid.NewGuid(), currency, amount, _now);
        
        balance.AddTransfer(subTransfer);

        balance.Amount.Should().Be(amount);
    }
    
    [Fact]
    public void deduct_funds_from_balance_should_succeed()
    {
        var currency = new Currency("EUR");
        var balance = Wallets.Domain.Wallets.Balance.Balance.Create(Guid.NewGuid(), currency, true, _now);
        var amount = new Amount(20);
        var incomingSubTransfer = new IncomingInternalTransfer(new TransferId(), new TransactionId(), Guid.NewGuid(), currency, amount, _now);
        var outgoingSubTransfer = new OutgoingInternalTransfer(new TransferId(), new TransactionId(), Guid.NewGuid(), currency, amount, _now);
        
        balance.AddTransfer(incomingSubTransfer);
        balance.AddTransfer(outgoingSubTransfer);

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