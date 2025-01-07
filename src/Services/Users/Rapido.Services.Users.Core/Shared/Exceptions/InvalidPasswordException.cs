using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Shared.Exceptions;

internal sealed class InvalidPasswordException : CustomException
{
    public InvalidPasswordException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}