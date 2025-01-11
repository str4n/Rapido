using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Testing.Database;
using Testcontainers.Consul;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;
using Xunit;

namespace Rapido.Framework.Testing;

public abstract class ApiTests<TApp, TContext> : IAsyncLifetime where TApp : class where TContext : DbContext
{
    private TestApp<TApp> _testApp;
    private readonly Func<DbContextOptions<TContext>, TContext> _createContext;
    private PostgreSqlContainer _postgres;
    // private ConsulContainer _consul;
    // private RedisContainer _redis;
    // private RabbitMqContainer _rabbitMq;
    protected HttpClient Client => _testApp.Client;
    protected TContext TestDbContext => GetDbContext();
    protected virtual Action<IServiceCollection> ConfigureServices => default;

    protected ApiTests(Func<DbContextOptions<TContext>, TContext> createContext)
        => _createContext = createContext;

    public async Task InitializeAsync()
    {
        _postgres = await TestDatabase<TContext>.InitPostgresAsync();
        //_consul = await TestServiceDiscovery.InitConsulAsync();
        //_redis = await TestCache.InitRedisAsync();
        //_rabbitMq = await TestMessageBroker.InitRabbitMqAsync();
        
        var connectionString = _postgres.GetConnectionString();
        
        await SeedAsync();
        
        var options = new Dictionary<string, string>
        {
            { "postgres:connectionString", connectionString },
            { "consul:discovery:register", false.ToString() },
            { "rabbitMQ:enabled", false.ToString() },
            { "vault:enabled", false.ToString() },
            { "logger:seq:enabled", false.ToString() }
        };
        _testApp = new TestApp<TApp>(ConfigureServices, options);
        AddClientHeaders();
    }

    protected virtual Task SeedAsync() => Task.CompletedTask;

    protected virtual void AddClientHeaders()
    {
    }

    public async Task DisposeAsync()
    {
        await _testApp.DisposeAsync();
        await _postgres.DisposeAsync();
    }
    
    private TContext GetDbContext() 
        => TestDatabase<TContext>.CreateDbContext(_createContext, _postgres.GetConnectionString());
}