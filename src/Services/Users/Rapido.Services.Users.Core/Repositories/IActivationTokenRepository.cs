using Rapido.Services.Users.Core.Entities.ActivationToken;

namespace Rapido.Services.Users.Core.Repositories;

internal interface IActivationTokenRepository
{
    Task<UserActivationToken> GetAsync(string token);
    Task<bool> ExistsAsync(string token);
    Task AddAsync(UserActivationToken token);
    Task DeleteAsync(UserActivationToken token);
}