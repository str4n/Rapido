namespace Rapido.Services.Wallets.Application.Wallets.DTO;

public sealed record WalletDto(
    Guid Id,
    Guid OwnerId, 
    IEnumerable<BalanceDto> Balances,
    IEnumerable<TransferDto> Transfers, 
    DateTime CreatedAt);