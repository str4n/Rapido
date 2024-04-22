using FluentAssertions;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Commands;
using Rapido.Services.Customers.Core.Commands.Handler;
using Rapido.Services.Customers.Core.EF.Repositories;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Entities.Lockout;
using Rapido.Services.Customers.Core.Repositories;
using Xunit;

namespace Rapido.Tests.Services.Customers.Integration.Commands;

public class LockCustomerTemporarilyTests : IDisposable
{
    private Task Act(LockCustomerTemporarily command) => _handler.HandleAsync(command);
    
    [Fact]
    public async Task given_valid_lock_customer_temporarily_command_should_succeed()
    {
        await _testDatabase.InitAsync();

        var id = Guid.Parse(Const.CompletedCustomerId);

        await Act(new LockCustomerTemporarily(id, "reason", "desc", DateTime.UtcNow));

        var customer = await _customerRepository.GetAsync(id);

        customer.State.Should().Be(CustomerState.Locked);
        customer.Lockouts.Should().ContainSingle();
        customer.Lockouts.Last().Should().BeOfType<TemporaryLockout>();
        ((TemporaryLockout)customer.Lockouts.Last()).IsActive(_clock.Now()).Should().BeTrue();
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange
    
    private readonly TestDatabase _testDatabase;
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;

    private LockCustomerTemporarilyHandler _handler;

    public LockCustomerTemporarilyTests()
    {
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        _clock = new TestClock();
        var messageBroker = new TestMessageBroker();

        _handler = new LockCustomerTemporarilyHandler(_testDatabase.DbContext, _clock, messageBroker);
    }
    
    #endregion Arrange
}