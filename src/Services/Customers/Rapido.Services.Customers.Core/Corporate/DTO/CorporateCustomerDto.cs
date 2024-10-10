using Rapido.Services.Customers.Core.Common.DTO;

namespace Rapido.Services.Customers.Core.Corporate.DTO;

public sealed record CorporateCustomerDto(Guid Id, string Email, string Name, string TaxId, 
    AddressDto Address, DateTime CreatedAt, 
    DateTime CompletedAt);