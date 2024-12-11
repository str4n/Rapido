using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class UserDeletedException(string message)
    : CustomException(message, ExceptionCategory.BadRequest);