namespace Rapido.Framework.HTTP.ServiceDiscovery;

public sealed class ConsulOptions
{
    public bool Enabled { get; set; }
    public string Url { get; set; }
    public ServiceRegistration Service { get; set; } = new();
    public HealthCheckRegistration HealthCheck { get; set; } = new();

    public sealed class ServiceRegistration
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
    
    public sealed class HealthCheckRegistration
    {
        public string Endpoint { get; set; }
        public TimeSpan Interval { get; set; }
        public TimeSpan DeregisterInterval { get; set; }
    }
}