using Rapido.Framework.Common.Exceptions;
using Rapido.Services.Users.Core.Repositories;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class CannotCreateActivationTokenException : CustomException
{
    public CannotCreateActivationTokenException(string message) : base(message, ExceptionCategory.BadRequest)
    {
    }
}