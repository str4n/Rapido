using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace Rapido.Framework.Testing.Cache;

public static class TestCache
{
    private const string Name = "redis-test";
    private const int Port = 6379;
    
    public static IDistributedCache CreateRedisCache(string connectionString)
    {
        var configurationOptions = ConfigurationOptions.Parse(connectionString);
        var connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);

        var redisCache = new RedisCache(new RedisCacheOptions
        {
            Configuration = connectionMultiplexer.Configuration,
            InstanceName = "redis"
        });

        return redisCache;
    }
    
    public static async Task<RedisContainer> InitRedisAsync()
    {
        var container = new RedisBuilder()
            .WithExposedPort(Port)
            .WithPortBinding(Port, true)
            .WithName($"{Name}-{Guid.NewGuid():N}")
            .Build();

        await container.StartAsync();
        return container;
    }
}