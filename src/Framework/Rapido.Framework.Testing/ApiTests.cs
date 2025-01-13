﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Testing.Cache;
using Rapido.Framework.Testing.Database;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Xunit;

namespace Rapido.Framework.Testing;

public abstract class ApiTests<TApp, TContext> : IAsyncLifetime where TApp : class where TContext : DbContext
{
    private TestApp<TApp> _testApp;
    private PostgreSqlContainer _postgres;
    private RedisContainer _redis;
    private readonly Func<DbContextOptions<TContext>, TContext> _createContext;
    private readonly ApiTestOptions _testOptions = new();
    protected HttpClient Client => _testApp.Client;
    protected TContext TestDbContext => GetDbContext();
    protected IDistributedCache Cache => GetCache();
    protected virtual Action<IServiceCollection> ConfigureServices => default;

    protected ApiTests(Func<DbContextOptions<TContext>, TContext> createContext, ApiTestOptions testOptions)
    {
        _createContext = createContext;
        _testOptions = testOptions;
    }

    protected ApiTests(Func<DbContextOptions<TContext>, TContext> createContext)
    {
        _createContext = createContext;
    }

    public async Task InitializeAsync()
    {
        var options = new Dictionary<string, string>
        {
            { "consul:discovery:register", false.ToString() },
            { "rabbitMQ:enabled", false.ToString() },
            { "vault:enabled", false.ToString() },
            { "logger:seq:enabled", false.ToString() }
        };
        
        if (_testOptions.EnablePostgres)
        {
            _postgres = await TestDatabase<TContext>.InitPostgresAsync();
            var connectionString = _postgres.GetConnectionString();
            options.Add("postgres:connectionString", connectionString);
        }

        if (_testOptions.EnableRedis)
        {
            _redis = await TestCache.InitRedisAsync();
            var connectionString = _redis.GetConnectionString();
            options.Add("redis:connectionString", connectionString);
        }
        
        await SeedAsync();
  
        _testApp = new TestApp<TApp>(ConfigureServices, options);
        
        foreach (var header in _testOptions.DefaultHttpClientHeaders)
        {
            Client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }
    
    //TODO: refactor seeding
    protected virtual Task SeedAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _testApp.DisposeAsync();
        await _postgres.DisposeAsync();
    }
    
    private TContext GetDbContext() 
        => TestDatabase<TContext>.CreateDbContext(_createContext, _postgres.GetConnectionString());

    private IDistributedCache GetCache()
        => TestCache.CreateRedisCache(_redis.GetConnectionString());
}