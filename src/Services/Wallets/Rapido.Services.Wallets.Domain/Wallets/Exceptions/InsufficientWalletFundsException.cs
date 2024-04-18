using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InsufficientWalletFundsException : CustomException
{
    public InsufficientWalletFundsException(Guid walletId) : base($"Insufficient funds for wallet with id: {walletId}", ExceptionCategory.BadRequest)
    {
    }
}