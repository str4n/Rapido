using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Shared.Exceptions;

internal sealed class UserNotActivatedException()
    : CustomException("User is not activated.", ExceptionCategory.BadRequest);