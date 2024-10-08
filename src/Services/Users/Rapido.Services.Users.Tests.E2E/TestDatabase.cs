using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Users.Core.EF;

namespace Rapido.Services.Users.Tests.E2E;

internal sealed class TestDatabase : IDisposable
{
    private readonly IClock _clock;
    public UsersDbContext DbContext { get; }

    public TestDatabase()
    {
        var connectionString = $"Host=localhost;Database=rapido-users-tests-{Guid.NewGuid():N};Username=postgres;Password=Admin12!";
        DbContext = new UsersDbContext(new DbContextOptionsBuilder<UsersDbContext>()
            .UseNpgsql(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging().Options);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        _clock = new TestClock();
    }

    public async Task InitAsync()
    {
        await DbContext.Database.MigrateAsync();
    }
    
    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}