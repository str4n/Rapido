namespace Rapido.Framework.Auth;

public sealed class AuthOptions
{
    public bool ValidateIssuer { get; set; } = true;
    public bool ValidateAudience { get; set; }
    public bool ValidateLifeTime { get; set; } = true;
    public bool ValidateTokenReply { get; set; }
    public bool RequireExpirationTime { get; set; } = true;
    public bool SaveSigninToken { get; set; }
    public bool SaveToken { get; set; } = true;
    public bool RequireAudience { get; set; } = true;
    public bool RequireHttpsMetadata { get; set; } = true;
    public bool RequireSignedTokens { get; set; } = true;
    public bool RefreshOnIssuerKeyNotFound { get; set; } = true;
    public bool IncludeErrorDetails { get; set; } = true;
    public bool ValidateIssuerSigningKey { get; set; } = true;
    public bool ValidateLifetime { get; set; } = true;
    public bool ValidateTokenReplay { get; set; }
    
    public string Authority { get; set; }
    public string MetadataAddress { get; set; }
    public string Issuer { get; set; }
    public string ValidIssuer { get; set; }
    public string Audience { get; set; }
    public string SigningKey { get; set; }
    public TimeSpan Expiry { get; set; }
}