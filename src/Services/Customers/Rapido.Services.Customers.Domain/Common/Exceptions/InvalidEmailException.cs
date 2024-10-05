using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Common.Exceptions;

internal sealed class InvalidEmailException : CustomException
{
    public InvalidEmailException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}