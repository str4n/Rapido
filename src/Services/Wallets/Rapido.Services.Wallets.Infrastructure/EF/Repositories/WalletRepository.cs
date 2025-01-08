using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Infrastructure.EF.Repositories;

internal sealed class WalletRepository(WalletsDbContext dbContext) : IWalletRepository
{
    public Task<Wallet> GetAsync(WalletId id, CancellationToken cancellationToken = default)
        => dbContext.Wallets
            .Include(x => x.Transfers)
            .Include(x => x.Balances)
            .ThenInclude(x => x.Transfers)
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<Wallet> GetAsync(OwnerId ownerId, CancellationToken cancellationToken = default)
        => dbContext.Wallets
            .Include(x => x.Transfers)
            .Include(x => x.Balances)
            .ThenInclude(x => x.Transfers)
            .SingleOrDefaultAsync(x => x.OwnerId == ownerId, cancellationToken);

    public async Task AddAsync(Wallet wallet, CancellationToken cancellationToken = default)
        => await dbContext.Wallets.AddAsync(wallet, cancellationToken);

    public Task UpdateAsync(Wallet wallet, CancellationToken cancellationToken = default)
        => Task.FromResult(dbContext.Wallets.Update(wallet));
}