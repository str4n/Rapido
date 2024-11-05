using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Application.Wallets.Exceptions;

internal sealed class CannotMakeSelfFundsTransferException : CustomException
{
    public CannotMakeSelfFundsTransferException() : base("Cannot transfer funds to the same wallet.", ExceptionCategory.BadRequest)
    {
    }
}