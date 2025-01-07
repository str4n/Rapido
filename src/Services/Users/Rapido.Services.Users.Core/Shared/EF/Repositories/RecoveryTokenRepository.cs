using Microsoft.EntityFrameworkCore;
using Rapido.Services.Users.Core.PasswordRecovery.Domain;
using Rapido.Services.Users.Core.PasswordRecovery.Repositories;

namespace Rapido.Services.Users.Core.Shared.EF.Repositories;

internal sealed class RecoveryTokenRepository(UsersDbContext dbContext) : IRecoveryTokenRepository
{
    public Task<PasswordRecoveryToken> GetAsync(string token)
        => dbContext.RecoveryTokens.SingleOrDefaultAsync(x => x.Token == token);

    public Task<bool> ExistsAsync(string token)
        => dbContext.RecoveryTokens.AnyAsync(x => x.Token == token);

    public async Task AddAsync(PasswordRecoveryToken token)
    {
        await dbContext.RecoveryTokens.AddAsync(token);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(PasswordRecoveryToken token)
    {
        dbContext.RecoveryTokens.Remove(token);
        await dbContext.SaveChangesAsync();
    }
}