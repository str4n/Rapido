using Rapido.Services.Wallets.Domain.Wallets.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public sealed record TransferName
{
    public string Value { get; }

    public TransferName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidTransferNameException("Transfer name cannot be empty.");
        }

        if (value.Length is < 3 or > 200)
        {
            throw new InvalidTransferNameException("Exceeded character limit");
        }

        Value = value;
    }

    public static implicit operator string(TransferName name) => name.Value;
    public static implicit operator TransferName(string name) => new(name);

    public override string ToString() => Value;
}