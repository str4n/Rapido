namespace Rapido.Framework.Redis.Cache;

public interface ICache
{
    Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default);
    Task DeleteAsync<T>(string key, CancellationToken cancellationToken = default);
}