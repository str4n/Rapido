using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class RoleNotFoundException : CustomException
{
    public RoleNotFoundException(string message) : base(message, ExceptionCategory.NotFound)
    {
    }
}