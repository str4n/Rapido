using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Customers.Core.Common.Commands;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.EF;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Tests.Integration.Commands;

public class LockCustomerPermanentlyHandlerTests : ApiTests<Program, CustomersDbContext>
{
    private Task Act(LockCustomerPermanently command) => Dispatcher.DispatchAsync(command);

    [Fact]
    public async Task given_valid_lock_customer_permanently_command_should_succeed()
    {
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var reason = "reason";
        var description = "description";

        var command = new LockCustomerPermanently(id, reason, description);

        await Act(command);

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

    public LockCustomerPermanentlyHandlerTests() : base(options => new CustomersDbContext(options))
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

        await dbContext.IndividualCustomers.AddAsync(new IndividualCustomer(Guid.Parse(Const.IndividualCustomerGuid),
            Const.IndividualCustomerEmail, clock.Now()));

        await dbContext.SaveChangesAsync();
    }
    
    #endregion Arrange

    
}