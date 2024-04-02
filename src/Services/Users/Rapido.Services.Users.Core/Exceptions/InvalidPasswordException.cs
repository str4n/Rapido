using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class InvalidPasswordException : CustomException
{
    public InvalidPasswordException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}