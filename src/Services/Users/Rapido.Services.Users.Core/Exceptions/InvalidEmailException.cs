using Rapido.Framework.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class InvalidEmailException : CustomException
{
    public InvalidEmailException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}