using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InvalidExchangeRateException : CustomException
{
    public InvalidExchangeRateException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}