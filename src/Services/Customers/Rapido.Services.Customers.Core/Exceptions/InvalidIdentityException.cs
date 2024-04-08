using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Exceptions;

internal sealed class InvalidIdentityException : CustomException
{
    public InvalidIdentityException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}