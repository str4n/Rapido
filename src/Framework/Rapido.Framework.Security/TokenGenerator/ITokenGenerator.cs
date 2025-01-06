namespace Rapido.Framework.Security.TokenGenerator;

public interface ITokenGenerator
{
    public const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    string Generate(int length = 24, string characters = Characters);
}