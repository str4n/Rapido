namespace Rapido.Framework.Security.TokenGenerator;

internal sealed class RandomTokenGenerator : ITokenGenerator
{
    public string Generate(int length = 24, string characters = ITokenGenerator.Characters)
        => new(Enumerable.Repeat(characters, length).Select(x => x[new Random().Next(x.Length)])
            .ToArray());
}