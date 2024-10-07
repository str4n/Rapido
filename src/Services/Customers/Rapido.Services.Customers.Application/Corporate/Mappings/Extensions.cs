using Rapido.Services.Customers.Application.Common.DTO;
using Rapido.Services.Customers.Application.Corporate.DTO;
using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Corporate.Customer;

namespace Rapido.Services.Customers.Application.Corporate.Mappings;

internal static class Extensions
{
    public static CorporateCustomerDto AsDto(this CorporateCustomer customer)
    {
        if (customer is null)
        {
            return null;
        }
        
        if (!customer.IsCompleted)
        {
            return new CorporateCustomerDto(customer.Id, customer.Email, default, 
                default, default, customer.CreatedAt, default);
        }
        
        return new CorporateCustomerDto(customer.Id, customer.Email, customer.Name, 
            customer.TaxId, customer.Address.AsDto(), customer.CreatedAt, customer.CompletedAt);
    }
    
    private static AddressDto AsDto(this Address address)
        => new AddressDto(address.Country, address.Province, address.City, address.Street, address.PostalCode);
}