using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Exceptions;

internal sealed class CannotUnlockCustomerException : CustomException
{
    public CannotUnlockCustomerException() : base("Customer is already locked.", ExceptionCategory.BadRequest)
    {
    }
}