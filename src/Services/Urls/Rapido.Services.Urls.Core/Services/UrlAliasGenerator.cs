namespace Rapido.Services.Urls.Core.Services;

internal sealed class UrlAliasGenerator : IUrlAliasGenerator
{
    private const string AvailableCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const int AliasLength = 5;
    
    public Task<string> Generate()
    {
        var alias = string.Empty;
            
        for (int i = 0; i < AliasLength; i++)
        {
            alias += AvailableCharacters[new Random().Next(AvailableCharacters.Length)];
        }

        return Task.FromResult(alias);
    }
}