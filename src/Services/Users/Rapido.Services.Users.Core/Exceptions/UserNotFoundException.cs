using Rapido.Framework.Base.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class UserNotFoundException : CustomException
{
    public UserNotFoundException(Guid userId) : base($"User with id: {userId} was not found.", ExceptionCategory.NotFound)
    {
    }
}