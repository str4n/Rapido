using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Common.Exceptions;

internal sealed class CustomerCompletedException : CustomException
{
    public CustomerCompletedException(Guid id) : base($"Customer with id: {id} is already completed.", ExceptionCategory.BadRequest)
    {
    }
}