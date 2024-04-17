using FluentAssertions;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Commands;
using Rapido.Services.Customers.Core.Commands.Handler;
using Rapido.Services.Customers.Core.EF.Repositories;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Repositories;
using Xunit;

namespace Rapido.Tests.Services.Customers.Integration.Commands;

public class CompleteCustomerHandlerTests : IDisposable
{
    private Task Act(CompleteCustomer command) => _handler.HandleAsync(command);

    [Fact]
    public async Task given_valid_create_customer_command_should_succeed()
    {
        await _testDatabase.InitAsync();

        var id = Guid.Parse(Const.Id);
        const string email = Const.EmailInUse;
        const string name = "Name";
        const string fullName = "full-name";
        const string country = "Poland";
        const string province = "Pomerania";
        const string city = "Gdansk";
        const string street = "Testowa 1";
        const string postalCode = "11-200";
        const string nationality = "PL";
        const string identityType = "Passport";
        const IdentityType identityTypeEnum = IdentityType.Passport;
        const string identitySeries = "32045934534534";

        await Act(new CompleteCustomer(id, name, fullName, country, province, city, street, postalCode, nationality, identityType, identitySeries));

        var customer = await _customerRepository.GetAsync(id);

        customer.Should().NotBeNull();

        customer.Name.Value.Should().Be(name.ToLowerInvariant());
        customer.FullName.Value.Should().Be(fullName.ToLowerInvariant());
        customer.Address.Country.Should().Be(country);
        customer.Address.Province.Should().Be(province);
        customer.Address.City.Should().Be(city);
        customer.Address.Street.Should().Be(street);
        customer.Address.PostalCode.Should().Be(postalCode);
        customer.Nationality.Value.Should().Be(nationality);
        customer.Identity.Type.Should().Be(identityTypeEnum);
        customer.Identity.Series.Should().Be(identitySeries);
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange
    
    private readonly TestDatabase _testDatabase;
    private readonly ICustomerRepository _customerRepository;

    private CompleteCustomerHandler _handler;

    public CompleteCustomerHandlerTests()
    {
        _testDatabase = new TestDatabase();
        _customerRepository = new CustomerRepository(_testDatabase.DbContext);
        var clock = new TestClock();
        var messageBroker = new TestMessageBroker();

        _handler = new CompleteCustomerHandler(_customerRepository, clock, messageBroker);
    }
    
    #endregion Arrange
}