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
using Rapido.Services.Customers.Core.Common.Exceptions;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;
using TestMessageBroker = Rapido.Framework.Testing.Abstractions.TestMessageBroker;

namespace Rapido.Services.Customers.Tests.Integration.Commands;

public class LockCustomerTemporarilyHandlerTests : ApiTests<Program, CustomersDbContext>
{
    private Task Act(LockCustomerTemporarily command) => Dispatcher.DispatchAsync(command);
    
    [Fact]
    public async Task given_valid_lock_customer_temporarily_command_should_succeed()
    {
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var reason = "reason";
        var description = "description";

        var endDate = _now.AddDays(1);

        var command = new LockCustomerTemporarily(id, reason, description, endDate);

        await Act(command);

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
    public async Task given_lock_customer_temporarily_command_with_invalid_end_date_should_throw_exception()
    {
        var id = Guid.Parse(Const.IndividualCustomerGuid);
        var reason = "reason";
        var description = "description";

        var endDate = _now.AddDays(-1);

        var command = new LockCustomerTemporarily(id, reason, description, endDate);

        var act = async () => await Act(command);

        await act.Should().ThrowAsync<CannotLockCustomerException>();
    }
    
    #region Arrange

    private readonly DateTime _now;

    public LockCustomerTemporarilyHandlerTests() : base(options => new CustomersDbContext(options))
    {
        _now = new TestClock().Now();
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

        await dbContext.IndividualCustomers.AddAsync(new IndividualCustomer(Guid.Parse(Const.IndividualCustomerGuid),
            Const.IndividualCustomerEmail, clock.Now()));

        await dbContext.SaveChangesAsync();
    }
    
    #endregion Arrange
}