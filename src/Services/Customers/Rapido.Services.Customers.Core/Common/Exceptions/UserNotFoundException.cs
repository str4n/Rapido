using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Exceptions;

internal sealed class UserNotFoundException : CustomException
{
    public UserNotFoundException(string message) : base(message, ExceptionCategory.NotFound)
    {
    }
}