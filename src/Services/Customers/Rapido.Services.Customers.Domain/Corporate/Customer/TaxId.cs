using Rapido.Services.Customers.Domain.Corporate.Exceptions;

namespace Rapido.Services.Customers.Domain.Corporate.Customer;

public sealed record TaxId
{
    public string Value { get; }

    public TaxId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidTaxIdException();
        }

        if (value.Length is > 20 or < 3)
        {
            throw new InvalidTaxIdException();
        }

        Value = value;
    }

    public static implicit operator string(TaxId name) => name.Value;
    public static implicit operator TaxId(string name) => new(name);

    public override string ToString() => Value;
}