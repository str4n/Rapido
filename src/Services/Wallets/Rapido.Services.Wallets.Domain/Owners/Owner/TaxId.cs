using Rapido.Services.Wallets.Domain.Owners.Exceptions;

namespace Rapido.Services.Wallets.Domain.Owners.Owner;

public sealed record TaxId
{
    public string Value { get; }

    public TaxId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidTaxIdException(value);
        }

        if (value.Length is > 20 or < 3)
        {
            throw new InvalidTaxIdException(value);
        }

        Value = value;
    }

    public static implicit operator string(TaxId name) => name.Value;
    public static implicit operator TaxId(string name) => new(name);

    public override string ToString() => Value;
}