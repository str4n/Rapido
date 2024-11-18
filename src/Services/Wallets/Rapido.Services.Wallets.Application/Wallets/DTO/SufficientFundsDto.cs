namespace Rapido.Services.Wallets.Application.Wallets.DTO;

public sealed record SufficientFundsDto(Guid WalletId, bool HasSufficientFunds);