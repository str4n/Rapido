using FluentAssertions;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.Domain.Repositories;
using Rapido.Services.Customers.Core.Common.EF.Repositories;
using Rapido.Services.Customers.Core.Corporate.Commands;
using Rapido.Services.Customers.Core.Corporate.Commands.Handlers;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;

namespace Rapido.Services.Customers.Tests.Integration.Commands;

public class CompleteCorporateCustomerHandlerTests : IDisposable
{
    private Task Act(CompleteCorporateCustomer command) => _handler.HandleAsync(command);
    
    [Fact]
    public async Task given_valid_complete_corporate_customer_command_should_succeed()
    {
        await _testDatabase.InitAsync();

        var id = Guid.Parse(Const.CorporateCustomerGuid);

        var name = new Name("cname");
        var taxId = new TaxId("4532-4234");
        var address = new Address("country", "province", "city", "street", "00-000");
        var nationality = new Nationality("PL");

        var command = new CompleteCorporateCustomer(id, name, taxId, 
            address.Country, address.Province, address.City, address.Street,
            address.PostalCode, nationality);

        await Act(command);

        var customer = await _customerRepository.GetCorporateCustomerAsync(id);

        customer.IsCompleted.Should().BeTrue();
        customer.CompletedAt.Should().Be(_now);

        customer.Name.Should().Be(name);
        customer.TaxId.Should().Be(taxId);
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

    private readonly CompleteCorporateCustomerHandler _handler;

    public CompleteCorporateCustomerHandlerTests()
    {
        var clock = new TestClock();
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        var messageBroker = new TestMessageBroker();
        _now = clock.Now();

        _handler = new CompleteCorporateCustomerHandler(_customerRepository, messageBroker, clock);
    }
    
    #endregion Arrange
}
