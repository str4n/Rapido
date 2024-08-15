namespace Rapido.Framework.Auth.ApiKeys.Vault;

public interface IApiKeyVault
{
    string GetExternalKey(string serviceName);
    string GetInternalKey(string serviceName);
    ValidationResult Validate(string apiKey);
}