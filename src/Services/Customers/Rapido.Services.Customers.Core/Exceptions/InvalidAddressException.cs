using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Exceptions;

internal sealed class InvalidAddressException : CustomException
{
    public InvalidAddressException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}