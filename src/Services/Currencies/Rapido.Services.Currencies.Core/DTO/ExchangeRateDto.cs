namespace Rapido.Services.Currencies.Core.DTO;

public sealed record ExchangeRateDto(string From, string To, double Rate);