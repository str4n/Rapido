using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Rapido.Framework.Testing.Database;

public static class TestDatabase<TContext> where TContext : DbContext
{
    private const string Name = "rapido-test";
    private const int Port = 5432;

    public static TContext CreateDbContext(Func<DbContextOptions<TContext>, TContext> createContext, string connectionString)
    {
        var options = new DbContextOptionsBuilder<TContext>().UseNpgsql(connectionString).Options;
        var context = createContext(options);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // var context = Activator.CreateInstance(typeof(TContext), args: options) as TContext;

        return context;
    }
    
    public static async Task<PostgreSqlContainer> InitPostgresAsync()
    {
        var container = new PostgreSqlBuilder()
            .WithExposedPort(Port)
            .WithPortBinding(Port, true)
            .WithName($"{Name}-{Guid.NewGuid():N}")
            .WithDatabase(Name)
            .WithUsername("test")
            .WithPassword("test")
            .Build();

        await container.StartAsync();
        return container;
    }
}