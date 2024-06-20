namespace Rapido.Services.Wallets.Application.Wallets.DTO;

public sealed record BalanceDto(Guid Id, Guid WalletId, double Amount, string Currency, bool IsPrimary, DateTime CreatedAt);