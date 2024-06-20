namespace Rapido.Services.Wallets.Application.Owners.DTO;

public sealed record CorporateOwnerDto(Guid Id, string Name, string TaxId, string State, 
    DateTime CreatedAt);