namespace Rapido.Services.Wallets.Domain.Wallets.Money;

public sealed record ExchangeRate(Currency From, Currency To, double Rate);