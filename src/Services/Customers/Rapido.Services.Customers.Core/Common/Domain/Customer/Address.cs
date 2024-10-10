using Rapido.Services.Customers.Core.Common.Domain.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Domain.Customer;

public sealed record Address
{
    public string Country { get; }
    public string Province { get; }
    public string City { get; }
    public string Street { get; }
    public string PostalCode { get; }

    public Address(string country, string province, string city, string street, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            throw new InvalidAddressException("Country cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(province))
        {
            throw new InvalidAddressException("Province cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(city))
        {
            throw new InvalidAddressException("City cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(street))
        {
            throw new InvalidAddressException("Street cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            throw new InvalidAddressException("Postal code cannot be empty.");
        }
        
        Country = country;
        Province = province;
        City = city;
        Street = street;
        PostalCode = postalCode;
    }

    private Address()
    {
    }

    public override string ToString() => $"{Street}, {PostalCode} {City}, {Country}";
}