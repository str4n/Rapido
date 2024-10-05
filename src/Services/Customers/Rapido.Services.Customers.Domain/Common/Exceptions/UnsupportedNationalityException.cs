using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Common.Exceptions;

internal sealed class UnsupportedNationalityException : CustomException
{
    public UnsupportedNationalityException() : base("Unsupported nationality code.", ExceptionCategory.ValidationError)
    {
    }
}