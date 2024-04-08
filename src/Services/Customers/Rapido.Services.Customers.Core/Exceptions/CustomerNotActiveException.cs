using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Exceptions;

internal sealed class CustomerNotActiveException : CustomException
{
    public CustomerNotActiveException() : base("Customer is not active.", ExceptionCategory.BadRequest)
    {
    }
}