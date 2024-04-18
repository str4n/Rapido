using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Domain.Wallets.Exceptions;

internal sealed class InvalidAggregateIdException : CustomException
{
    public InvalidAggregateIdException(Guid value) : base($"AggregateId: {value} is invalid.", ExceptionCategory.ValidationError)
    {
    }
}