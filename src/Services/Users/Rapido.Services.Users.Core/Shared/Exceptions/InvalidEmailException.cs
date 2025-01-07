using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Shared.Exceptions;

internal sealed class InvalidEmailException : CustomException
{
    public InvalidEmailException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}