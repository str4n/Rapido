using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Customers.Core.Common.Commands;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.EF;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Tests.Integration.Endpoints;

public class LockCustomerPermanentlyEndpointTests()
    : ApiTests<Program, CustomersDbContext>(options => new CustomersDbContext(options))
{
    private Task<HttpResponseMessage> Act(LockCustomerPermanently command) 
        => Client.PostAsJsonAsync($"/customers/lock/perm/{command.CustomerId}", command);

    [Fact]
    public async Task given_valid_lock_customer_permanently_request_should_succeed()
    {
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var reason = "reason";
        var description = "description";

        var command = new LockCustomerPermanently(id, reason, description);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var customer = await TestDbContext.IndividualCustomers
            .Include(x => x.Lockouts)
            .SingleOrDefaultAsync(x => x.Id == id);

        customer.IsLocked.Should().BeTrue();
        customer.Lockouts.Should().NotBeEmpty();
        var lockout = customer.Lockouts.Last();

        lockout.Should().NotBeNull();
        lockout.Should().BeOfType<PermanentLockout>();
    }
    
    
    #region Arrange

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };
    
    protected override async Task SeedAsync()
    {
        var dbContext = TestDbContext;
        await dbContext.Database.MigrateAsync();
        
        var clock = new TestClock();

        await dbContext.IndividualCustomers.AddAsync(new IndividualCustomer(Guid.Parse(Const.IndividualCustomerGuid),
            Const.IndividualCustomerEmail, clock.Now()));

        await dbContext.SaveChangesAsync();
    }
    
    protected override void AddClientHeaders()
    {
        var jwt = new TestAuthenticator()
            .GenerateJwt(Guid.Parse(Const.IndividualCustomerGuid), "admin", Const.IndividualCustomerEmail);
        
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
    }
    
    #endregion Arrange

    
}