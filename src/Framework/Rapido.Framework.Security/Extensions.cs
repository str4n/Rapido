using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Security.Hasher;
using Rapido.Framework.Security.Vault;

namespace Rapido.Framework.Security;

public static class Extensions
{
    public static WebApplicationBuilder AddSecurity(this WebApplicationBuilder builder)
    {
        builder.AddVault();

        builder.Services.AddSingleton<IHasher, Hasher.Hasher>();

        return builder;
    }
}