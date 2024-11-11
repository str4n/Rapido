
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.EF;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;
using Rapido.Services.Customers.Core.Individual.Commands;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Tests.Integration.Commands;

public class CompleteIndividualCustomerHandlerTests : ApiTests<Program, CustomersDbContext>
{
    private Task Act(CompleteIndividualCustomer command) => Dispatcher.DispatchAsync(command);
    
    [Fact]
    public async Task given_valid_complete_individual_customer_command_should_succeed()
    {
        var id = Guid.Parse(Const.IndividualCustomerGuid);

        var name = new Name("iname");
        var fullName = new FullName("full name");
        var address = new Address("country", "province", "city", "street", "00-000");
        var nationality = new Nationality("PL");

        var command = new CompleteIndividualCustomer(id, name, fullName, 
            address.Country, address.Province, address.City, address.Street,
            address.PostalCode, nationality);

        await Act(command);

        var customer = await TestDbContext.IndividualCustomers.SingleOrDefaultAsync(x => x.Id == id);

        customer.Should().NotBeNull();
        customer.IsCompleted.Should().BeTrue();

        customer.Name.Should().Be(name);
        customer.FullName.Should().Be(fullName);
        customer.Address.Should().Be(address);
        customer.Nationality.Should().Be(nationality);
    }
    
    
    #region Arrange

    public CompleteIndividualCustomerHandlerTests() : base(options => new CustomersDbContext(options))
    {
    }
    
    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };
    
    protected override async Task SeedAsync()
    {
        var dbContext = GetDbContext();
        await dbContext.Database.MigrateAsync();
        
        var clock = new TestClock();

        await dbContext.IndividualCustomers.AddAsync(new IndividualCustomer(Guid.Parse(Const.IndividualCustomerGuid),
            Const.IndividualCustomerEmail, clock.Now()));

        await dbContext.SaveChangesAsync();
    }
    
    #endregion Arrange
}