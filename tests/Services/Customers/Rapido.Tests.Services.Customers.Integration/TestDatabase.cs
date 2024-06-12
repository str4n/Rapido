using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Customers.Core.EF;
using Rapido.Services.Customers.Core.Entities.Customer;

namespace Rapido.Tests.Services.Customers.Integration;

internal sealed class TestDatabase : IDisposable
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
        
        var customer = new Customer(Guid.Parse(Const.Id), Const.EmailInUse, CustomerType.Individual ,_clock.Now());
        var completedCustomer = new Customer(Guid.Parse(Const.CompletedCustomerId), Const.CompletedCustomerEmail, CustomerType.Individual ,_clock.Now());
        
        completedCustomer.Complete("namex", "full-namex", 
            new Address("country", "province", "city", "street", "postalCode"), "PL", 
            new Identity("Passport", "342423423"), _clock.Now());
        
        await DbContext.Customers.AddAsync(completedCustomer);
        await DbContext.Customers.AddAsync(customer);
        await DbContext.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}