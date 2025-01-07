using Rapido.Framework.Security.TokenGenerator;
using Rapido.Services.Users.Core.UserActivation.Repositories;

namespace Rapido.Services.Users.Core.UserActivation.Services;

internal sealed class ActivationTokenGenerator(IActivationTokenRepository repository, ITokenGenerator tokenGenerator) 
    : IActivationTokenGenerator
{
    public async Task<string> GenerateActivationToken()
    {
        var token = string.Empty;
        var tokenIsValid = false;
        
        while (!tokenIsValid)
        {
            token = tokenGenerator.Generate(8);
            if (!await repository.ExistsAsync(token))
            {
                tokenIsValid = true;
            }
        }

        return token;
    }
}