using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.EF;
using Rapido.Services.Customers.Core.Corporate.Commands;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;

namespace Rapido.Services.Customers.Tests.Integration.Commands;

public class CompleteCorporateCustomerHandlerTests : ApiTests<Program, CustomersDbContext>
{
    private async Task Act(CompleteCorporateCustomer command) => await Dispatcher.DispatchAsync(command);
    
    [Fact]
    public async Task given_valid_complete_corporate_customer_command_should_succeed()
    {
        var id = Guid.Parse(Const.CorporateCustomerGuid);

        var name = new Name("cname");
        var taxId = new TaxId("4532-4234");
        var address = new Address("country", "province", "city", "street", "00-000");
        var nationality = new Nationality("PL");

        var command = new CompleteCorporateCustomer(id, name, taxId, 
            address.Country, address.Province, address.City, address.Street,
            address.PostalCode, nationality);

        await Act(command);

        var customer = await TestDbContext.CorporateCustomers.SingleOrDefaultAsync(x => x.Id == id);
        
        customer.Should().NotBeNull();
        customer.IsCompleted.Should().BeTrue();

        customer.Name.Should().Be(name);
        customer.TaxId.Should().Be(taxId);
        customer.Address.Should().Be(address);
        customer.Nationality.Should().Be(nationality);
    }
    
    
    #region Arrange

    public CompleteCorporateCustomerHandlerTests() : base(options => new CustomersDbContext(options))
    {
    }
    
    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };
    
    protected override async Task SeedAsync()
    {
        var dbContext = TestDbContext;
        await dbContext.Database.MigrateAsync();
        
        var clock = new TestClock();

        await dbContext.CorporateCustomers.AddAsync(new CorporateCustomer(Guid.Parse(Const.CorporateCustomerGuid),
           Const.CorporateCustomerEmail, clock.Now()));

        await dbContext.SaveChangesAsync();
    }
    
    #endregion Arrange
}
