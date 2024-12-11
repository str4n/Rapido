using Rapido.Services.Users.Core.Repositories;

namespace Rapido.Services.Users.Core.Services;

internal sealed class ActivationTokenGenerator(IActivationTokenRepository repository) : IActivationTokenGenerator
{
    public async Task<string> GenerateActivationToken()
    {
        var token = string.Empty;
        var tokenIsValid = false;
        
        while (!tokenIsValid)
        {
            token = GenerateToken();
            if (!await repository.ExistsAsync(token))
            {
                tokenIsValid = true;
            }
        }

        return token;
    }

    private string GenerateToken()
        => $"{Guid.NewGuid():N}";
}