using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Shared.Exceptions;

internal sealed class InvalidCredentialsException : CustomException
{
    public InvalidCredentialsException() : base("Invalid email or password.", ExceptionCategory.ValidationError)
    {
    }
}