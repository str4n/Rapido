namespace Rapido.Framework.Security.Hasher;

public interface IHasher
{
    string Sha256(string value);
}