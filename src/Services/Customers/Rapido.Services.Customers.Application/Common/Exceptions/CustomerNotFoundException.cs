using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Application.Common.Exceptions;

internal sealed class CustomerNotFoundException : CustomException
{
    public CustomerNotFoundException(Guid id) : base($"Customer with id: {id} was not found.", ExceptionCategory.NotFound)
    {
    }
}