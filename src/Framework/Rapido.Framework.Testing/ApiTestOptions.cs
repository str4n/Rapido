namespace Rapido.Framework.Testing;

public sealed class ApiTestOptions
{
    public bool EnablePostgres => true;
    public bool EnableRedis => false;
    public Dictionary<string, string> DefaultHttpClientHeaders = new();
}