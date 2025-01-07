using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Shared.Exceptions;

internal sealed class InvalidActivationTokenException(string message) 
    : CustomException(message, ExceptionCategory.BadRequest);