using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class InvalidAccountTypeException : CustomException
{
    public InvalidAccountTypeException() : base("Invalid account type.", ExceptionCategory.ValidationError)
    {
    }
}