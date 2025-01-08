using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Infrastructure.EF.Repositories;

internal sealed class CorporateOwnerRepository(WalletsDbContext dbContext) : ICorporateOwnerRepository
{
    public Task<CorporateOwner> GetAsync(OwnerId id, CancellationToken cancellationToken = default)
        => dbContext.CorporateOwners.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IEnumerable<CorporateOwner>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.CorporateOwners.ToListAsync(cancellationToken);

    public async Task AddAsync(CorporateOwner owner, CancellationToken cancellationToken = default)
        => await dbContext.CorporateOwners.AddAsync(owner, cancellationToken);

    public Task UpdateAsync(CorporateOwner owner, CancellationToken cancellationToken = default)
        => Task.FromResult(dbContext.CorporateOwners.Update(owner));
}