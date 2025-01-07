using Rapido.Services.Users.Core.PasswordRecovery.Domain;

namespace Rapido.Services.Users.Core.PasswordRecovery.Repositories;

public interface IRecoveryTokenRepository
{
    Task<PasswordRecoveryToken> GetAsync(string token);
    Task<bool> ExistsAsync(string token);
    Task AddAsync(PasswordRecoveryToken token);
    Task DeleteAsync(PasswordRecoveryToken token);
}