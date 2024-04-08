using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Exceptions;

internal sealed class InvalidFullNameException : CustomException
{
    public InvalidFullNameException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}