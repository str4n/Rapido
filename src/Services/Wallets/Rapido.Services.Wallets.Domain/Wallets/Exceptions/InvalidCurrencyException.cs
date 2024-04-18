using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InvalidCurrencyException : CustomException
{
    public InvalidCurrencyException() : base("Currency is invalid.", ExceptionCategory.ValidationError)
    {
    }
}