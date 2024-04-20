using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Wallets.Application.Owners.Exceptions;

internal sealed class CorporateOwnerAlreadyExists : CustomException
{
    public CorporateOwnerAlreadyExists(Guid id) : base($"Corporate owner with id: {id} already exists.", ExceptionCategory.AlreadyExists)
    {
    }
}