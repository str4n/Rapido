using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Exceptions;

internal sealed class CannotVerifyAccountException : CustomException
{
    public CannotVerifyAccountException() : base("Cannot verify customer account.", ExceptionCategory.BadRequest)
    {
    }
}