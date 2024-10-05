using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Common.Exceptions;

internal sealed class InvalidAddressException : CustomException
{
    public InvalidAddressException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}