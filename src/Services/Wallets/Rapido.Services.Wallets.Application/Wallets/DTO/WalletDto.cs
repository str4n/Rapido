namespace Rapido.Services.Wallets.Application.Wallets.DTO;

public sealed record WalletDto(
    Guid Id,
    Guid OwnerId, 
    double Amount, 
    string Currency, 
    IEnumerable<TransferDto> Transfers, 
    DateTime CreatedAt);