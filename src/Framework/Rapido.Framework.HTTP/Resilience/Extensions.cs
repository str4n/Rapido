using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace Rapido.Framework.HTTP.Resilience;

public static class Extensions
{
    public static void AddHttpClientResilience(this IServiceCollection services)
    {
        services.ConfigureHttpClientDefaults(http =>
        {
            http.AddStandardResilienceHandler();
        });
    }
    // => builder.AddStandardResilienceHandler();
    // .AddResilienceHandler("resilience", p =>
    // {
    //     p.AddRetry(new HttpRetryStrategyOptions
    //     {
    //         MaxRetryAttempts = 3,
    //         BackoffType = DelayBackoffType.Exponential,
    //         UseJitter = true,
    //         Delay = TimeSpan.FromMilliseconds(500)
    //     });
    //
    //     p.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
    //     {
    //         SamplingDuration = TimeSpan.FromSeconds(10),
    //         FailureRatio = 0.9,
    //         MinimumThroughput = 5,
    //         BreakDuration = TimeSpan.FromSeconds(5)
    //     });
    //     
    //     p.AddTimeout(TimeSpan.FromSeconds(1));
    // });
}