using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Application.Wallets.Exceptions;

internal sealed class OwnerNotFoundException : CustomException
{
    public OwnerNotFoundException(string name) : base($"Owner with name: {name} was not found.", ExceptionCategory.NotFound)
    {
    }
}