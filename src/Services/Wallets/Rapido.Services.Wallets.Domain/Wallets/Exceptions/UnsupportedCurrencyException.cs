using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class UnsupportedCurrencyException : CustomException
{
    public UnsupportedCurrencyException() : base("Unsupported currency.", ExceptionCategory.BadRequest)
    {
    }
}