using Rapido.Services.Wallets.Domain.Owners.Exceptions;

namespace Rapido.Services.Wallets.Domain.Owners.Owner;

public sealed record OwnerFullName
{
    public string Value { get; }

    public OwnerFullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOwnerFullNameException();
        }

        if (value.Length is < 3 or > 100)
        {
            throw new InvalidOwnerFullNameException();
        }

        Value = value;
    }

    public static implicit operator string(OwnerFullName name) => name.Value;
    public static implicit operator OwnerFullName(string name) => new(name);

    public override string ToString() => Value;
}