namespace Rapido.Services.Customers.Application.Common.DTO;

public sealed record AddressDto(string Country, string Province, string City, string Street, string PostalCode);