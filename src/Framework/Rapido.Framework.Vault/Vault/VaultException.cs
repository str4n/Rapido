using Rapido.Framework.Common.Exceptions;

namespace Rapido.Framework.Vault.Vault;

internal sealed class VaultException : CustomException
{
    public VaultException(string message) : base(message, ExceptionCategory.InternalError)
    {
    }
}