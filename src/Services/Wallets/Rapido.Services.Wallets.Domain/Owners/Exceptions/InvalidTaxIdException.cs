using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Owners.Exceptions;

internal sealed class InvalidTaxIdException : CustomException
{
    public InvalidTaxIdException(string taxId) : base($"Tax id: {taxId} is invalid.", ExceptionCategory.ValidationError)
    {
    }
}