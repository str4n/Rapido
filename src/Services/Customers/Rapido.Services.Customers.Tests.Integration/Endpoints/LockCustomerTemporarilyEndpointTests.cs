using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Customers.Core.Common.Commands;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.EF;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;
using TestMessageBroker = Rapido.Framework.Testing.Abstractions.TestMessageBroker;

namespace Rapido.Services.Customers.Tests.Integration.Endpoints;

public class LockCustomerTemporarilyEndpointTests()
    : ApiTests<Program, CustomersDbContext>(options => new CustomersDbContext(options))
{
    private Task<HttpResponseMessage> Act(LockCustomerTemporarily command) 
        => Client.PostAsJsonAsync($"/customers/lock/temp/{command.CustomerId}", command);
    
    [Fact]
    public async Task given_valid_lock_customer_temporarily_request_should_succeed()
    {
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var reason = "reason";
        var description = "description";

        var endDate = _now.AddDays(1);

        var command = new LockCustomerTemporarily(id, reason, description, endDate);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var customer = await TestDbContext.IndividualCustomers
            .Include(x => x.Lockouts)
            .SingleOrDefaultAsync(x => x.Id == id);

        customer.IsLocked.Should().BeTrue();
        customer.Lockouts.Should().NotBeEmpty();
        var lockout = customer.Lockouts.Last();

        lockout.Should().NotBeNull();
        lockout.Should().BeOfType<TemporaryLockout>();

        var tempLockout = (TemporaryLockout)lockout;

        tempLockout.EndDate.Should().Be(endDate);
    }
    
    [Fact]
    public async Task given_lock_customer_temporarily_request_with_invalid_end_date_should_return_bad_request_status_code()
    {
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var reason = "reason";
        var description = "description";

        var endDate = _now.AddDays(-1);

        var command = new LockCustomerTemporarily(id, reason, description, endDate);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    #region Arrange

    private readonly DateTime _now = new TestClock().Now();

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
        s.AddScoped<IClock, TestClock>();
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