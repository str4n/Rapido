using Rapido.Framework.Base.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class UserNotActiveException : CustomException
{
    public UserNotActiveException(string email) : base($"User with email: {email} is not active.", ExceptionCategory.BadRequest)
    {
    }
}