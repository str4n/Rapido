using Rapido.Services.Wallets.Domain.Owners.Exceptions;

namespace Rapido.Services.Wallets.Domain.Owners.Owner;

public sealed record OwnerName
{
    public string Value { get; }

    public OwnerName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOwnerNameException();
        }

        if (value.Length is < 3 or > 100)
        {
            throw new InvalidOwnerNameException();
        }

        Value = value;
    }

    public static implicit operator string(OwnerName name) => name.Value;
    public static implicit operator OwnerName(string name) => new(name);

    public override string ToString() => Value;
}