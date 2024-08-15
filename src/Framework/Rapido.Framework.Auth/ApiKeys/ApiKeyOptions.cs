namespace Rapido.Framework.Auth.ApiKeys;

public sealed class ApiKeyOptions
{
    public IEnumerable<ApiKey> External { get; set; } = new List<ApiKey>();
    public IEnumerable<ApiKey> Internal { get; set; } = new List<ApiKey>();
}