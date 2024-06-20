using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InvalidExchangeException : CustomException
{
    public InvalidExchangeException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}