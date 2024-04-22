namespace Rapido.Services.Wallets.Application.Wallets.DTO;

public sealed record TransferDto(
    Guid Id, 
    string Name, 
    string Description, 
    double Amount, 
    string Currency, 
    string Type, 
    DateTime CreatedAt);