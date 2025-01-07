using Rapido.Framework.Security.TokenGenerator;
using Rapido.Services.Users.Core.PasswordRecovery.Repositories;

namespace Rapido.Services.Users.Core.PasswordRecovery.Services;

internal sealed class RecoveryTokenGenerator(IRecoveryTokenRepository repository, ITokenGenerator tokenGenerator) 
    : IRecoveryTokenGenerator
{
    public async Task<string> GenerateRecoveryToken()
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