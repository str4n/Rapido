using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Shared.Exceptions;

internal sealed class CannotCreateActivationTokenException : CustomException
{
    public CannotCreateActivationTokenException(string message) : base(message, ExceptionCategory.BadRequest)
    {
    }
}