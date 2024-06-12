using FluentAssertions;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Commands;
using Rapido.Services.Customers.Core.Commands.Handler;
using Rapido.Services.Customers.Core.EF.Repositories;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Repositories;
using Xunit;

namespace Rapido.Tests.Services.Customers.Integration.Commands;

public class CreateCustomerHandlerTests : IDisposable
{
    private Task Act(CreateCustomer command) => _handler.HandleAsync(command);

    [Fact]
    public async Task given_valid_create_customer_command_should_succeed()
    {
        await _testDatabase.InitAsync();

        var email = Const.EmailNotInUse;
        var customerType = CustomerType.Individual;

        await Act(new CreateCustomer(email, customerType.ToString()));

        var customer = await _customerRepository.GetAsync(email);
        customer.Should().NotBeNull();
        customer.Email.Value.Should().Be(email);
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange
    
    private readonly TestDatabase _testDatabase;
    private readonly ICustomerRepository _customerRepository;

    private CreateCustomerHandler _handler;

    public CreateCustomerHandlerTests()
    {
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        var clock = new TestClock();
        var userApiClient = new TestUserApiClient();

        _handler = new CreateCustomerHandler(_customerRepository, clock, userApiClient);
    }
    
    #endregion Arrange
}