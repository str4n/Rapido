using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Application.Common.Exceptions;

internal sealed class UserNotFoundException : CustomException
{
    public UserNotFoundException(string message) : base(message, ExceptionCategory.NotFound)
    {
    }
}