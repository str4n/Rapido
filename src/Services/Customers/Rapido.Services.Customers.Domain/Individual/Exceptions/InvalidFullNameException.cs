using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Individual.Exceptions;

internal sealed class InvalidFullNameException : CustomException
{
    public InvalidFullNameException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}