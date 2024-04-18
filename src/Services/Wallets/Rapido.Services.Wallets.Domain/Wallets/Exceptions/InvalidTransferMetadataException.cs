using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InvalidTransferMetadataException : CustomException
{
    public InvalidTransferMetadataException() : base("Transfer metadata is invalid.", ExceptionCategory.ValidationError)
    {
    }
}