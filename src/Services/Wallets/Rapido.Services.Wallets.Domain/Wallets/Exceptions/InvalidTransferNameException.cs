using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InvalidTransferNameException : CustomException
{
    public InvalidTransferNameException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}