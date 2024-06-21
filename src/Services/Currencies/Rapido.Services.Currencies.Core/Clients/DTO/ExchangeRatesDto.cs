using System.Text.Json.Serialization;

namespace Rapido.Services.Currencies.Core.Clients.DTO;

internal sealed record ExchangeRatesDto
{
    [JsonPropertyName("result")]
    public string Result { get; init; }
    
    [JsonPropertyName("time_last_update_unix")]
    public long TimeLastUpdateUnix { get; init; }
    
    [JsonPropertyName("time_last_update_utc")]
    public string TimeLastUpdateUtc { get; init; }
    
    [JsonPropertyName("time_next_update_unix")]
    public long TimeNextUpdateUnix { get; init; }
    
    [JsonPropertyName("time_next_update_utc")]
    public string TimeNextUpdateUtc { get; init; }
    
    [JsonPropertyName("base_code")]
    public string BaseCode { get; init; }
    
    [JsonPropertyName("conversion_rates")]
    public ConversionRateDto ConversionRates { get; init; }
    
    internal sealed record ConversionRateDto
    {
        public double USD { get; init; }
        public double EUR { get; init; }
        public double PLN { get; init; }
        public double GBP { get; init; }
    }
}
