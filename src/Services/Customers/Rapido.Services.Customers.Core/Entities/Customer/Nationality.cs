using Rapido.Services.Customers.Core.Exceptions;

namespace Rapido.Services.Customers.Core.Entities.Customer;

internal sealed record Nationality
{
    private static readonly HashSet<string> AllowedValue = new()
    {
        "PL", "GB", "US", "DE", "FR", "ES", "NO", "CZ", "EE", "DK", "BE", "UA", "CA", "PT", "SK", "SE", "HR", "EL", "AT",
        "HU", "LU", "LT", "NL", "CY", "LV"
    };
    
    public string Value { get; }

    public Nationality(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidNationalityException("Nationality cannot be empty.");
        }

        if (!AllowedValue.Contains(value))
        {
            throw new UnsupportedNationalityException();
        }

        Value = value;
    }
    
    public static implicit operator string(Nationality value) => value.Value;
    public static implicit operator Nationality(string value) => new(value);

    public override string ToString() => Value;
}