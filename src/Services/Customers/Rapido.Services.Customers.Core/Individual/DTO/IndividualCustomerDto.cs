using Rapido.Services.Customers.Core.Common.DTO;

namespace Rapido.Services.Customers.Core.Individual.DTO;

public sealed record IndividualCustomerDto(Guid Id, string Email, string Name, string FullName, 
    AddressDto Address, DateTime CreatedAt, 
    DateTime CompletedAt);