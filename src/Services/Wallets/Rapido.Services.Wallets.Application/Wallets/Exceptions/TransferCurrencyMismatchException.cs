using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Application.Wallets.Exceptions;

internal sealed class TransferCurrencyMismatchException : CustomException
{
    public TransferCurrencyMismatchException(string message) 
        : base(message, ExceptionCategory.BadRequest)
    {
    }
}