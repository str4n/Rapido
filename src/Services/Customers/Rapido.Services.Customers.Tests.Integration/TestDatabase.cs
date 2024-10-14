using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.EF;
using Rapido.Services.Customers.Core.Corporate.Domain.Customer;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Tests.Integration;

internal sealed class TestDatabase
{
    private readonly IClock _clock;
    public CustomersDbContext DbContext { get; }
    
    public TestDatabase()
    {
        var connectionString = $"Host=localhost;Database=rapido-customers-tests-{Guid.NewGuid():N};Username=postgres;Password=Admin12!";
        DbContext = new CustomersDbContext(new DbContextOptionsBuilder<CustomersDbContext>()
            .UseNpgsql(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging().Options);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        _clock = new TestClock();
    }
    
    public async Task InitAsync()
    {
        await DbContext.Database.MigrateAsync();

        var customers = new List<Customer>
        {
            new CorporateCustomer(Guid.Parse(Const.CorporateCustomerGuid), Const.CorporateCustomerEmail,
                _clock.Now()),

            new IndividualCustomer(Guid.Parse(Const.IndividualCustomerGuid), Const.IndividualCustomerEmail,
                _clock.Now())
        };

        await DbContext.Customers.AddRangeAsync(customers);

        await DbContext.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}