namespace Rapido.Services.Wallets.Application.Owners.DTO;

public sealed record IndividualOwnerDto(Guid Id, string Name, string FullName, string State, 
    DateTime CreatedAt, DateTime VerifiedAt);