using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class InvalidActivationTokenException(string message) 
    : CustomException(message, ExceptionCategory.BadRequest);