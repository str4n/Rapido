using Rapido.Services.Wallets.Domain.Wallets.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public sealed record TransferMetadata
{
    public string Value { get; }
        
    public TransferMetadata(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidTransferMetadataException();
        }
            
        if (value.Length > 1000)
        {
            throw new InvalidTransferMetadataException();
        }

        Value = value.Trim();
    }

    public static implicit operator string(TransferMetadata value) => value.Value;
    public static implicit operator TransferMetadata(string value) => new(value);

    public override string ToString() => Value;
}