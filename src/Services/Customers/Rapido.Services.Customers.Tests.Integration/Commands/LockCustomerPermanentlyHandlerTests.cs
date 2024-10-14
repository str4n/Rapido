using FluentAssertions;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Common.Commands;
using Rapido.Services.Customers.Core.Common.Commands.Handlers;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.EF.Repositories;

namespace Rapido.Services.Customers.Tests.Integration.Commands;

public class LockCustomerPermanentlyHandlerTests : IDisposable
{
    private Task Act(LockCustomerPermanently command) => _handler.HandleAsync(command);

    [Fact]
    public async Task given_valid_lock_customer_permanently_command_should_succeed()
    {
        await _testDatabase.InitAsync();
        
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var reason = "reason";
        var description = "description";

        var command = new LockCustomerPermanently(id, reason, description);

        await Act(command);

        var customer = await _customerRepository.GetCustomerAsync(id);

        customer.IsLocked.Should().BeTrue();
        customer.Lockouts.Should().NotBeEmpty();
        var lockout = customer.Lockouts.Last();

        lockout.Should().NotBeNull();
        lockout.Should().BeOfType<PermanentLockout>();
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly ICustomerRepository _customerRepository;
    private readonly DateTime _now;

    private readonly LockCustomerPermanentlyHandler _handler;

    public LockCustomerPermanentlyHandlerTests()
    {
        var clock = new TestClock();
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        var messageBroker = new TestMessageBroker();
        _now = clock.Now();

        _handler = new LockCustomerPermanentlyHandler(_customerRepository, clock, messageBroker);
    }
    
    #endregion Arrange
}