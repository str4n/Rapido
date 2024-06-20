using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Application.Wallets.Exceptions;

internal sealed class WalletNotFoundException : CustomException
{
    public WalletNotFoundException() : base("Wallet was not found.", ExceptionCategory.NotFound)
    {
    }
}