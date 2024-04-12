using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Exceptions;

internal sealed class CustomerAlreadyExistsException : CustomException
{
    public CustomerAlreadyExistsException(string message) : base(message, ExceptionCategory.AlreadyExists)
    {
    }
}