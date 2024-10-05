using Rapido.Services.Customers.Application.Common.DTO;

namespace Rapido.Services.Customers.Application.Individual.DTO;

public sealed record IndividualCustomerDto(Guid Id, string Email, string Name, string FullName, 
    AddressDto Address, DateTime CreatedAt, 
    DateTime CompletedAt);