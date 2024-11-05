using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class NegativeAmountException : CustomException
{
    public NegativeAmountException() : base("Amount cannot be negative.", ExceptionCategory.ValidationError)
    {
    }
}