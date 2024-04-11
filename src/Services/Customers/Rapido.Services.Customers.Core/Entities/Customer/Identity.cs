using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Rapido.Services.Customers.Core.Exceptions;

namespace Rapido.Services.Customers.Core.Entities.Customer;

internal sealed record Identity
{
    public IdentityType Type { get; }
    public string Series { get; }

    public Identity(string type, string series)
    {
        if (!Enum.TryParse(type, out IdentityType typeEnum))
        {
            throw new InvalidIdentityException("Unsupported identity type.");
        }

        if (string.IsNullOrWhiteSpace(series))
        {
            throw new InvalidIdentityException("Series cannot be empty.");
        }

        Type = typeEnum;
        Series = series;
    }

    private Identity()
    {
    }

    public static implicit operator string(Identity identity) => $"{identity.Type},{identity.Series}";
    public static implicit operator Identity(string identity) => new(Convert(identity));
    
    public override string ToString() => $"{Type},{Series}";

    private static Identity Convert(string value)
    {
        var values = value.Split(",");

        return new Identity(values[0], values[1]);
    }
}

internal enum IdentityType
{
    Passport,
    IdCart,
    DriverLicense
}