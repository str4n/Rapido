using Rapido.Services.Currencies.Core.DTO;

namespace Rapido.Services.Currencies.Core.Services;

public interface IExchangeRateService
{
    Task<IEnumerable<ExchangeRateDto>> GetExchangeRates();
}