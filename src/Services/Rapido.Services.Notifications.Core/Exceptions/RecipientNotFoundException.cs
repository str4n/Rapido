using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Notifications.Core.Exceptions;

internal sealed class RecipientNotFoundException : CustomException
{
    public RecipientNotFoundException(Guid id) : base($"Recipient with id: {id} was not found.", ExceptionCategory.NotFound)
    {
    }
}