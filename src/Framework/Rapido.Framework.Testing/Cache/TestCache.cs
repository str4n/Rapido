using Testcontainers.Redis;

namespace Rapido.Framework.Testing.Cache;

public static class TestCache
{
    private const string Name = "redis-test";
    private const int Port = 6379;
    
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