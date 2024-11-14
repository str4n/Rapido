using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Customers.Core.Common.Commands;
using Rapido.Services.Customers.Core.Common.Domain.Exceptions;
using Rapido.Services.Customers.Core.Common.Domain.Lockout;
using Rapido.Services.Customers.Core.Common.EF;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Tests.Integration.Commands;

public class UnlockCustomerHandlerTests : ApiTests<Program, CustomersDbContext>
{
    private Task Act(UnlockCustomer command) => Dispatcher.DispatchAsync(command);
    
    [Fact]
    public async Task given_valid_unlock_customer_command__customer_with_perm_lock_should_succeed()
    {
        var dbContext = TestDbContext;
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var clock = new TestClock();
        var customer = await dbContext.IndividualCustomers
            .Include(x => x.Lockouts)
            .SingleOrDefaultAsync(x => x.Id == id);
        customer.Lock(new PermanentLockout(id, "test", "test", clock.Now()));
        dbContext.IndividualCustomers.Update(customer);
        await dbContext.SaveChangesAsync();
        
        var command = new UnlockCustomer(id);

        await Act(command);
        
        customer = await TestDbContext.IndividualCustomers
            .Include(x => x.Lockouts)
            .SingleOrDefaultAsync(x => x.Id == id);
        
        customer.IsLocked.Should().BeFalse();
        var lockout = customer.Lockouts.Last();
        lockout.Should().NotBeNull();
        lockout.Should().BeOfType<PermanentLockout>();
        ((PermanentLockout)lockout).IsActive().Should().BeFalse();
    }
    
    [Fact]
    public async Task given_valid_unlock_customer_command__customer_with_temp_lock_should_succeed()
    {
        var dbContext = TestDbContext;
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var now = new TestClock().Now();
        var endDate = now.AddDays(1);
        var customer = await dbContext.IndividualCustomers
            .Include(x => x.Lockouts)
            .SingleOrDefaultAsync(x => x.Id == id);
        customer.Lock(new TemporaryLockout(id, "test", "test", now, endDate));
        dbContext.IndividualCustomers.Update(customer);
        await dbContext.SaveChangesAsync();
        
        var command = new UnlockCustomer(id);

        await Act(command);
        
        customer = await TestDbContext.IndividualCustomers
            .Include(x => x.Lockouts)
            .SingleOrDefaultAsync(x => x.Id == id);
        
        
        customer.IsLocked.Should().BeFalse();
        var lockout = customer.Lockouts.Last();
        lockout.Should().NotBeNull();
        lockout.Should().BeOfType<TemporaryLockout>();
        ((TemporaryLockout)lockout).EndDate.Should().Be(now);
    }

    [Fact]
    public async Task unlock_not_locked_customer_should_throw_exception()
    {
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var command = new UnlockCustomer(id);

        var act = async () => await Act(command);

        await act.Should().ThrowAsync<CannotUnlockCustomerException>();
    }
    
    
    #region Arrange

    public UnlockCustomerHandlerTests() : base(options => new CustomersDbContext(options))
    {
    }
    
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

        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var customer = new IndividualCustomer(id,
            Const.IndividualCustomerEmail, clock.Now());

        await dbContext.IndividualCustomers.AddAsync(customer);

        await dbContext.SaveChangesAsync();
    }

    #endregion
}