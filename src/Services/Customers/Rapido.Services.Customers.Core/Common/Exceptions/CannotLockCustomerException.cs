using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Exceptions;

internal sealed class CannotLockCustomerException : CustomException
{
    public CannotLockCustomerException(string message) : base(message, ExceptionCategory.BadRequest)
    {
    }
}