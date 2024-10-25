using Rapido.Services.Wallets.Domain.Wallets.Money;

namespace Rapido.Services.Wallets.Application.Wallets.DTO;

public sealed record WalletDto(
    Guid Id,
    Guid OwnerId, 
    IEnumerable<BalanceDto> Balances,
    IEnumerable<TransferDto> Transfers, 
    double TotalBalance,
    DateTime CreatedAt);