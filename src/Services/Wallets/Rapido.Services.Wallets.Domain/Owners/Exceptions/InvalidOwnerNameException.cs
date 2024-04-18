using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Owners.Exceptions;

internal sealed class InvalidOwnerNameException : CustomException
{
    public InvalidOwnerNameException() : base("Owner name is invalid.", ExceptionCategory.ValidationError)
    {
    }
}