﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Rapido.Framework.Common.Time;

namespace Rapido.Framework.Redis.Cache;

internal sealed class RedisCache : ICache
{
    private readonly IDistributedCache _cache;
    private readonly IClock _clock;

    public RedisCache(IDistributedCache cache, IClock clock)
    {
        _cache = cache;
        _clock = clock;
    }
    
    public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return default;
        }

        var value = await _cache.GetStringAsync(key, cancellationToken);

        return string.IsNullOrWhiteSpace(value) ? default : JsonConvert.DeserializeObject<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        if (expiry is null)
        {
            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), cancellationToken);
            return;
        }

        await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = new DateTimeOffset(_clock.Now()).Add((TimeSpan)expiry)
        }, cancellationToken);
    }

    public async Task DeleteAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        await _cache.RemoveAsync(key, cancellationToken);
    }
}