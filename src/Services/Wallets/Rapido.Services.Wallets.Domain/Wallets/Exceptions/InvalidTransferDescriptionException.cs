using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InvalidTransferDescriptionException : CustomException
{
    public InvalidTransferDescriptionException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}