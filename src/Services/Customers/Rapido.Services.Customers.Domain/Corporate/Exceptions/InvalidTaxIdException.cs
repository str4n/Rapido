using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Corporate.Exceptions;

internal sealed class InvalidTaxIdException : CustomException
{
    public InvalidTaxIdException() : base("Tax id is invalid.", ExceptionCategory.ValidationError)
    {
    }
}