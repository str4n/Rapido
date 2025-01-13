namespace Rapido.Framework.Testing;

public sealed class ApiTestOptions
{
    public bool EnablePostgres { get; set; } = true;
    public bool EnableRedis { get; set; } = false;
    public Dictionary<string, string> DefaultHttpClientHeaders = new();
}