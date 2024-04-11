using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Exceptions;

internal sealed class CustomerAlreadyExistsException : CustomException
{
    public CustomerAlreadyExistsException(string name) : base($"Customer with name: {name} already exists.", ExceptionCategory.AlreadyExists)
    {
    }
}