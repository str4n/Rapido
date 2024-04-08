using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Exceptions;

internal sealed class InvalidNationalityException : CustomException
{
    public InvalidNationalityException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}