using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Shared.Exceptions;

internal sealed class UserAlreadyActivatedException()
    : CustomException("User was already activated.", ExceptionCategory.BadRequest);