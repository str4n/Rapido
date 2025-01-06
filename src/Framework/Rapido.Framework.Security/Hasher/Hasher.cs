using System.Security.Cryptography;
using System.Text;

namespace Rapido.Framework.Security.Hasher;

internal sealed class Hasher : IHasher
{
    public string Sha256(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));

        var sb = new StringBuilder();
        
        foreach (var @byte in bytes)
        {
            sb.Append(@byte.ToString("X"));
        }

        return sb.ToString();
    }
}