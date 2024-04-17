using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Commands;
using Rapido.Services.Customers.Core.Commands.Handler;
using Rapido.Services.Customers.Core.EF.Repositories;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Repositories;
using Xunit;

namespace Rapido.Tests.Services.Customers.Integration.Commands;

public class VerifyCustomerHandlerTests : IDisposable
{
    private Task Act(VerifyCustomer command) => _handler.HandleAsync(command);
    
    [Fact]
    public async Task given_valid_verify_customer_command_should_succeed()
    {
        await _testDatabase.InitAsync();
        
        var id = Guid.Parse(Const.CompletedCustomerId);

        await Act(new VerifyCustomer(id));

        var customer = await _customerRepository.GetAsync(id);

        customer.State.Should().Be(CustomerState.Verified);
        customer.VerifiedAt.Should().Be(_clock.Now());
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange
    
    private readonly TestDatabase _testDatabase;
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;

    private VerifyCustomerHandler _handler;

    public VerifyCustomerHandlerTests()
    {
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        _clock = new TestClock();
        var messageBroker = new TestMessageBroker();

        _handler = new VerifyCustomerHandler(_customerRepository, _clock, messageBroker);
    }
    
    #endregion Arrange
}