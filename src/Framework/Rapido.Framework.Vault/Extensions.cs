using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Rapido.Framework.Common;
using Rapido.Framework.Common.Serialization;
using Rapido.Framework.Vault.Vault;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Rapido.Framework.Vault;

public static class Extensions
{
    private const string SectionName = "Vault";
    
    public static WebApplicationBuilder AddVault(this WebApplicationBuilder builder)
    {
        var options = builder.Configuration.BindOptions<VaultOptions>(SectionName);

        if (!options.Enabled)
        {
            return builder;
        }

        var (client, settings) = GetClientAndSettings(options);

        builder.Services.AddSingleton(client);
        builder.Services.AddSingleton(settings);
        builder.Services.AddTransient<IKeyValueSecrets, KeyValueSecrets>();

        builder.Configuration.AddVaultAsync(options).GetAwaiter().GetResult();

        return builder;
    }

    private static async Task AddVaultAsync(this IConfigurationBuilder builder, VaultOptions options)
    {
        var (client, _) = GetClientAndSettings(options);
        if (options.KV.Enabled)
        {
            var kvPath = options.KV.Path;
            
            if (string.IsNullOrWhiteSpace(kvPath))
            {
                throw new VaultException("KV path is missing.");
            }

            var jsonSerializer = new NewtonsoftJsonSerializer();
            var keyValueSecrets = new KeyValueSecrets(client, new OptionsWrapper<VaultOptions>(options), jsonSerializer);
            var parser = new JsonParser();
            var secret = (await keyValueSecrets.GetAsync(kvPath))["data"];
            var json = JsonSerializer.Serialize(secret);
            var data = parser.Parse(json);
            var source = new MemoryConfigurationSource
            {
                InitialData = data
            };

            builder.Add(source);
        }
    }

    private static (IVaultClient client, VaultClientSettings settings) GetClientAndSettings(VaultOptions options)
    {
        var settings = new VaultClientSettings(options.Url, GetAuthMethod(options));
        var client = new VaultClient(settings);

        return (client, settings);
    }

    private static IAuthMethodInfo GetAuthMethod(VaultOptions options)
        => options.Authentication.Type switch
        {
            AuthenticationType.Token => new TokenAuthMethodInfo(options.Authentication.Token.Token),
            
            AuthenticationType.UserPass => new UserPassAuthMethodInfo(options.Authentication.UserPass.Username,
                    options.Authentication.UserPass.Password),
            
            AuthenticationType.None => throw new VaultException("Vault authentication type must be set."),
            
            _ => throw new VaultException("Invalid vault authentication type.")
        };
}