using FluentAssertions;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Commands;
using Rapido.Services.Customers.Core.Commands.Handler;
using Rapido.Services.Customers.Core.EF.Repositories;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Repositories;
using Xunit;

namespace Rapido.Tests.Services.Customers.Integration.Commands;

public class LockCustomerHandlerTests : IDisposable
{
    private Task Act(LockCustomer command) => _handler.HandleAsync(command);
    
    [Fact]
    public async Task given_valid_lock_customer_command_should_succeed()
    {
        await _testDatabase.InitAsync();
        
        var id = Guid.Parse(Const.Id);
        
        await Act(new LockCustomer(id, "reason", "desc"));

        var customer = await _customerRepository.GetAsync(id);

        customer.Lockouts.Should().NotBeEmpty();
        customer.State.Should().Be(CustomerState.Locked);
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange
    
    private readonly TestDatabase _testDatabase;
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;

    private LockCustomerHandler _handler;

    public LockCustomerHandlerTests()
    {
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        _clock = new TestClock();
        var messageBroker = new TestMessageBroker();

        _handler = new LockCustomerHandler(_testDatabase.DbContext, _clock, messageBroker);
    }
    
    #endregion Arrange
}