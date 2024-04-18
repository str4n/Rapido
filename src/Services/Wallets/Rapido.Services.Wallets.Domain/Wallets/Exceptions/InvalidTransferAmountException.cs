using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InvalidTransferAmountException : CustomException
{
    public InvalidTransferAmountException(double value) : base($"Transfer has invalid amount: {value}" , ExceptionCategory.BadRequest)
    {
    }
}