using Rapido.Services.Customers.Core.Individual.Domain.Exceptions;

namespace Rapido.Services.Customers.Core.Individual.Domain.Customer;

public sealed record FullName
{
    public string Value { get; }

    public FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidFullNameException("Name cannot be empty.");
        }

        if (value.Length is > 200 or < 3)
        {
            throw new InvalidFullNameException("Name length must be between 3 - 200 letters.");
        }

        Value = value;
    }

    public static implicit operator string(FullName name) => name.Value;
    public static implicit operator FullName(string name) => new(name);
    
    public override string ToString() => Value;
}