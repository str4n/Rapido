using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Infrastructure.EF.Repositories;

internal sealed class WalletRepository : IWalletRepository
{
    private readonly WalletsDbContext _dbContext;

    public WalletRepository(WalletsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Wallet> GetAsync(WalletId id)
        => _dbContext.Wallets
            .Include(x => x.Transfers)
            .Include(x => x.Balances)
            .ThenInclude(x => x.Transfers)
            .SingleOrDefaultAsync(x => x.Id == id);

    public Task<Wallet> GetAsync(OwnerId ownerId)
        => _dbContext.Wallets
            .Include(x => x.Transfers)
            .Include(x => x.Balances)
            .ThenInclude(x => x.Transfers)
            .SingleOrDefaultAsync(x => x.OwnerId == ownerId);

    public async Task AddAsync(Wallet wallet)
    {
        await _dbContext.Wallets.AddAsync(wallet);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Wallet wallet)
    {
        _dbContext.Wallets.Update(wallet);
        await _dbContext.SaveChangesAsync();
    }
}