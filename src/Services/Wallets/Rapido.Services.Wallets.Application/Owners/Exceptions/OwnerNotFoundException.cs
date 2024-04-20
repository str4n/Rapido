using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Application.Owners.Exceptions;

internal sealed class OwnerNotFoundException : CustomException
{
    public OwnerNotFoundException(Guid id) : base($"Owner with id: {id} was not found.", ExceptionCategory.NotFound)
    {
    }
}