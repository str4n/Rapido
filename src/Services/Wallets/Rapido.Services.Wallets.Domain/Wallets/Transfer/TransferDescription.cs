using Rapido.Services.Wallets.Domain.Wallets.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public sealed record TransferDescription
{
    public string Value { get; }

    public TransferDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidTransferDescriptionException("Transfer description cannot be empty.");
        }

        if (value.Length is < 3 or > 500)
        {
            throw new InvalidTransferDescriptionException("Exceeded character limit");
        }

        Value = value;
    }

    public static implicit operator string(TransferDescription name) => name.Value;
    public static implicit operator TransferDescription(string name) => new(name);

    public override string ToString() => Value;
}