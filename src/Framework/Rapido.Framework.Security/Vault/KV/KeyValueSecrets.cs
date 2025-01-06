using Microsoft.Extensions.Options;
using Rapido.Framework.Common.Serialization;
using VaultSharp;

namespace Rapido.Framework.Security.Vault.KV;

internal sealed class KeyValueSecrets(IVaultClient client, IOptions<VaultOptions> options, IJsonSerializer jsonSerializer) : IKeyValueSecrets
{
    public async Task<T> GetAsync<T>(string path)
        => jsonSerializer.Deserialize<T>(jsonSerializer.Serialize(await GetAsync(path)));

    public async Task<IDictionary<string, object>> GetAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new VaultException("Vault KV secret path cannot be empty.");
        }

        try
        {
            return (await client.V1.Secrets.KeyValue.V1.ReadSecretAsync($"{path}", mountPoint: options.Value.KV.MountPoint)).Data;
        }
        catch (Exception e)
        {
            throw new VaultException($"Cannot get vault secret for path '{path}'. {e.Message}");
        }
    }
}