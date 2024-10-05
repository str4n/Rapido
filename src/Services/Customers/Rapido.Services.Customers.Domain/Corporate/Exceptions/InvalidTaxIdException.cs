using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Corporate.Exceptions;

internal sealed class InvalidTaxIdException : CustomException
{
    public InvalidTaxIdException(string taxId) : base($"Tax id: {taxId} is invalid.", ExceptionCategory.ValidationError)
    {
    }
}