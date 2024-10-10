using FluentAssertions;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.EF.Repositories;
using Rapido.Services.Customers.Core.Individual.Commands;
using Rapido.Services.Customers.Core.Individual.Commands.Handlers;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Tests.Integration.Commands;

public class CompleteIndividualCustomerHandlerTests : IDisposable
{
    private Task Act(CompleteIndividualCustomer command) => _handler.HandleAsync(command);
    
    [Fact]
    public async Task given_valid_complete_individual_customer_command_should_succeed()
    {
        await _testDatabase.InitAsync();

        var id = Guid.Parse(Const.IndividualCustomerGuid);

        var name = new Name("testname");
        var fullName = new FullName("full name");
        var address = new Address("country", "province", "city", "street", "00-000");
        var nationality = new Nationality("PL");

        var command = new CompleteIndividualCustomer(id, name, fullName, 
            address.Country, address.Province, address.City, address.Street,
            address.PostalCode, nationality);

        await Act(command);

        var customer = await _customerRepository.GetIndividualCustomerAsync(id);

        customer.IsCompleted.Should().BeTrue();
        customer.CompletedAt.Should().Be(_now);

        customer.Name.Should().Be(name);
        customer.FullName.Should().Be(fullName);
        customer.Address.Should().Be(address);
        customer.Nationality.Should().Be(nationality);
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly ICustomerRepository _customerRepository;
    private readonly DateTime _now;

    private readonly CompleteIndividualCustomerHandler _handler;

    public CompleteIndividualCustomerHandlerTests()
    {
        var clock = new TestClock();
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        var messageBroker = new TestMessageBroker();
        _now = clock.Now();

        _handler = new CompleteIndividualCustomerHandler(_customerRepository, clock, messageBroker);
    }
    
    #endregion Arrange
}