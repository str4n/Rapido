using Microsoft.EntityFrameworkCore;
using Rapido.Services.Users.Core.UserActivation.Domain;
using Rapido.Services.Users.Core.UserActivation.Repositories;

namespace Rapido.Services.Users.Core.Shared.EF.Repositories;

internal sealed class ActivationTokenRepository(UsersDbContext dbContext) : IActivationTokenRepository
{
    public Task<UserActivationToken> GetAsync(string token)
        => dbContext.ActivationTokens.SingleOrDefaultAsync(x => x.Token == token);

    public Task<bool> ExistsAsync(string token)
        => dbContext.ActivationTokens.AnyAsync(x => x.Token == token);

    public async Task AddAsync(UserActivationToken token)
    {
        await dbContext.ActivationTokens.AddAsync(token);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserActivationToken token)
    {
        dbContext.ActivationTokens.Remove(token);
        await dbContext.SaveChangesAsync();
    }
}