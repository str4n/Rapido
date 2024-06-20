using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class BalanceAlreadyExistsException : CustomException
{
    public BalanceAlreadyExistsException(string currency) : base($"Balance in {currency} already exists.", ExceptionCategory.BadRequest)
    {
    }
}