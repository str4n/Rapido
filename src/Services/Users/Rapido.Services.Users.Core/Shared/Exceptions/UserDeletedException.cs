using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Shared.Exceptions;

internal sealed class UserDeletedException(string message)
    : CustomException(message, ExceptionCategory.BadRequest);