using Microsoft.Extensions.Options;

namespace Rapido.Framework.Auth.ApiKeys.Vault;

internal sealed class UserSecretsApiKeyVault : IApiKeyVault
{
    private readonly IEnumerable<ApiKey> _externalApiKeys;
    private readonly IEnumerable<ApiKey> _internalApiKeys;
    
    public UserSecretsApiKeyVault(IOptions<ApiKeyOptions> options)
    {
        _externalApiKeys = options.Value.External;
        _internalApiKeys = options.Value.Internal;
    }

    public string GetInternalKey(string serviceName)
        => _internalApiKeys.SingleOrDefault(x => x.ServiceName == serviceName)?.Key;
    
    public string GetExternalKey(string serviceName)
        => _externalApiKeys.SingleOrDefault(x => x.ServiceName == serviceName)?.Key;

    public ValidationResult Validate(string apiKey)
    {
        var key = _internalApiKeys.SingleOrDefault(x => x.Key == apiKey);

        if (key is null)
        {
            return new ValidationResult(false, string.Empty);
        }

        return new ValidationResult(true, key.ServiceName);
    }
}