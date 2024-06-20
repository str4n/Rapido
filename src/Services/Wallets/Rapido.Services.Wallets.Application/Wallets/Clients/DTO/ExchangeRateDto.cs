namespace Rapido.Services.Wallets.Application.Wallets.Clients.DTO;

public sealed record ExchangeRateDto(string From, string To, double Rate);