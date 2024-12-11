using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class UserAlreadyActivatedException(string email)
    : CustomException($"User with email: {email} was already activated.", ExceptionCategory.BadRequest);