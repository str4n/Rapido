using Rapido.Framework.Base.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class InvalidCredentialsException : CustomException
{
    public InvalidCredentialsException() : base("Invalid credentials", ExceptionCategory.ValidationError)
    {
    }
}