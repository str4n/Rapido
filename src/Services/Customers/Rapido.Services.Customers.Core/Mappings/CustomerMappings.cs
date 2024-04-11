using Rapido.Services.Customers.Core.DTO;
using Rapido.Services.Customers.Core.Entities.Customer;

namespace Rapido.Services.Customers.Core.Mappings;

internal static class CustomerMappings
{
    public static CustomerDto AsDto(this Customer customer)
        => new CustomerDto(customer.Id, customer.Email, customer.Name, customer.FullName,
            new AddressDto(customer.Address.Country, customer.Address.Province, customer.Address.City,
                customer.Address.Street, customer.Address.PostalCode), 
            new IdentityDto(customer.Identity.Type.ToString(), customer.Identity.Series), 
            customer.State.ToString(), customer.CompletedAt, customer.CompletedAt, customer.VerifiedAt);
}