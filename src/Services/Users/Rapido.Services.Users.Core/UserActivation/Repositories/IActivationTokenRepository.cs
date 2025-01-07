using Rapido.Services.Users.Core.UserActivation.Domain;

namespace Rapido.Services.Users.Core.UserActivation.Repositories;

internal interface IActivationTokenRepository
{
    Task<UserActivationToken> GetAsync(string token);
    Task<bool> ExistsAsync(string token);
    Task AddAsync(UserActivationToken token);
    Task DeleteAsync(UserActivationToken token);
}