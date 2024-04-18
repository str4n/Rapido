using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Owners.Exceptions;

internal sealed class InvalidOwnerFullNameException : CustomException
{
    public InvalidOwnerFullNameException() : base("Owner full name is invalid.", ExceptionCategory.ValidationError)
    {
    }
}