using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.DTO;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;
using Rapido.Services.Customers.Core.Individual.DTO;

namespace Rapido.Services.Customers.Core.Individual.Mappings;

internal static class Extensions
{
    public static IndividualCustomerDto AsDto(this IndividualCustomer customer)
    {
        if (customer is null)
        {
            return null;
        }
        
        if (!customer.IsCompleted)
        {
            return new IndividualCustomerDto(customer.Id, customer.Email, default, 
                default, default, customer.CreatedAt, default);
        }
        
        return new IndividualCustomerDto(customer.Id, customer.Email, customer.Name, 
            customer.FullName, customer.Address.AsDto(), customer.CreatedAt, customer.CompletedAt);
    }

    private static AddressDto AsDto(this Address address)
        => new AddressDto(address.Country, address.Province, address.City, address.Street, address.PostalCode);
}