using FluentAssertions;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Common.Commands;
using Rapido.Services.Customers.Core.Common.Commands.Handlers;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.EF.Repositories;
using Rapido.Services.Customers.Core.Common.Exceptions;

namespace Rapido.Services.Customers.Tests.Integration.Commands;

public class LockCustomerTemporarilyHandlerTests : IDisposable
{
    private Task Act(LockCustomerTemporarily command) => _handler.HandleAsync(command);
    
    [Fact]
    public async Task given_valid_lock_customer_temporarily_command_should_succeed()
    {
        await _testDatabase.InitAsync();
        
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var reason = "reason";
        var description = "description";

        var endDate = _now.AddDays(1);

        var command = new LockCustomerTemporarily(id, reason, description, endDate);

        await Act(command);

        var customer = await _customerRepository.GetCustomerAsync(id);

        customer.IsLocked.Should().BeTrue();
        customer.Lockouts.Should().NotBeEmpty();
        var lockout = customer.Lockouts.Last();

        lockout.Should().NotBeNull();
        lockout.Should().BeOfType<TemporaryLockout>();

        var tempLockout = (TemporaryLockout)lockout;

        tempLockout.EndDate.Should().Be(endDate);
    }
    
    [Fact]
    public async Task given_lock_customer_temporarily_command_with_invalid_end_date_should_throw_exception()
    {
        await _testDatabase.InitAsync();
        
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var reason = "reason";
        var description = "description";

        var endDate = _now.AddDays(-1);

        var command = new LockCustomerTemporarily(id, reason, description, endDate);

        var act = async () => await Act(command);

        await act.Should().ThrowAsync<CannotLockCustomerException>();
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly ICustomerRepository _customerRepository;
    private readonly DateTime _now;

    private readonly LockCustomerTemporarilyHandler _handler;

    public LockCustomerTemporarilyHandlerTests()
    {
        var clock = new TestClock();
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        var messageBroker = new TestMessageBroker();
        _now = clock.Now();

        _handler = new LockCustomerTemporarilyHandler(_customerRepository, clock, messageBroker);
    }
    
    #endregion Arrange
}