namespace Rapido.Framework.Vault.Vault;

public interface IKeyValueSecrets
{
    Task<T> GetAsync<T>(string path);
    Task<IDictionary<string, object>> GetAsync(string path);
}