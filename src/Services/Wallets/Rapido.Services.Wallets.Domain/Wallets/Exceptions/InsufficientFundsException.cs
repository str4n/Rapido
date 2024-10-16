using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InsufficientFundsException : CustomException
{
    public InsufficientFundsException(Guid walletId) : base($"Insufficient funds for wallet with id: {walletId}", ExceptionCategory.BadRequest)
    {
    }
}