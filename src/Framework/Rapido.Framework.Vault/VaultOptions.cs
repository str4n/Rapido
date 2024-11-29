namespace Rapido.Framework.Vault;

public sealed class VaultOptions
{
    public bool Enabled { get; set; }
    public string Url { get; set; }
    public AuthenticationOptions Authentication { get; set; } = new();
    public KeyValueOptions KV { get; set; } = new();
}

public sealed class KeyValueOptions
{
    public bool Enabled { get; set; }
    public string MountPoint { get; set; } = "secret/data/";
    public string Path { get; set; }
}

public sealed class AuthenticationOptions
{
    public AuthenticationType Type { get; set; } = AuthenticationType.None;
    public TokenOptions Token { get; set; } = new();
    public UserPass UserPass { get; set; } = new();
}

public sealed class TokenOptions
{
    public string Token { get; set; }
}

public sealed class UserPass 
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public enum AuthenticationType
{
    None,
    Token,
    UserPass
}