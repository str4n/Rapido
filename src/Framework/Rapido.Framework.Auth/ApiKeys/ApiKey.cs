namespace Rapido.Framework.Auth.ApiKeys;

public sealed class ApiKey
{
    public const string HeaderName = "X-API-Key";
    public string ServiceName { get; set; }
    public string Key { get; set; }
}