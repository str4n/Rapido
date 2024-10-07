using Rapido.Services.Customers.Application.Common.DTO;

namespace Rapido.Services.Customers.Application.Corporate.DTO;

public sealed record CorporateCustomerDto(Guid Id, string Email, string Name, string TaxId, 
    AddressDto Address, DateTime CreatedAt, 
    DateTime CompletedAt);