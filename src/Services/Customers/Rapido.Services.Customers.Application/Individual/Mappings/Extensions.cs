using Rapido.Services.Customers.Application.Common.DTO;
using Rapido.Services.Customers.Application.Individual.DTO;
using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Individual.Customer;

namespace Rapido.Services.Customers.Application.Individual.Mappings;

internal static class Extensions
{
    public static IndividualCustomerDto AsDto(this IndividualCustomer customer)
    {
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