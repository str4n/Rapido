using Rapido.Services.Customers.Core.Common.Domain.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Domain.Customer;

public sealed record Name
{
    public string Value { get; }

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidNameException("Name cannot be empty.");
        }

        if (value.Length is > 50 or < 3)
        {
            throw new InvalidNameException("Name length must be between 3 - 50 letters.");
        }

        Value = value.Trim().ToLowerInvariant().Replace(" ", "-");
    }

    public static implicit operator string(Name name) => name.Value;
    public static implicit operator Name(string name) => new(name);
    
    public override string ToString() => Value;
}