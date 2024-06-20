namespace Rapido.Services.Customers.Core.DTO;

public record CustomerDto(Guid Id, string Email, string Name, string FullName, 
    AddressDto Address, IdentityDto Identity, string State, DateTime CreatedAt, 
    DateTime CompletedAt);