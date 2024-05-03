namespace Rapido.Framework.Redis.Cache;

public interface ICache
{
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task DeleteAsync<T>(string key);
}