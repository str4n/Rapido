using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Application.Wallets.Exceptions;

internal sealed class WalletAlreadyExistsException : CustomException
{
    public WalletAlreadyExistsException(Guid ownerId, string currency) : base($"Owner with id: {ownerId} already has wallet supporting {currency}.", ExceptionCategory.AlreadyExists)
    {
    }
}