using Testcontainers.Consul;

namespace Rapido.Framework.Testing.ServiceDiscovery;

public static class TestServiceDiscovery
{
    private const string Name = "consul-test";
    private const int Port = 8500;
    
    public static async Task<ConsulContainer> InitConsulAsync()
    {
        var container = new ConsulBuilder()
            .WithImage("hashicorp/consul:latest")
            .WithExposedPort(Port)
            .WithName($"{Name}-{Guid.NewGuid():N}")
            .Build();

        await container.StartAsync();
        return container;
    }
}