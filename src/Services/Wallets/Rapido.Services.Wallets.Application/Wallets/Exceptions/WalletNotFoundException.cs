using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Application.Wallets.Exceptions;

internal sealed class WalletNotFoundException : CustomException
{
    public WalletNotFoundException(Guid id) : base($"Wallet with id: {id} was not found.", ExceptionCategory.NotFound)
    {
    }
}