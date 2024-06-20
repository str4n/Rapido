using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class BalanceNotFoundException : CustomException
{
    public BalanceNotFoundException() : base("Balance was not found.", ExceptionCategory.NotFound)
    {
    }
}