using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class UserNotActivatedException()
    : CustomException("User is not activated.", ExceptionCategory.BadRequest);