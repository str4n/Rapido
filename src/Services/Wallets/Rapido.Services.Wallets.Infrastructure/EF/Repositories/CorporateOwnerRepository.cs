using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Infrastructure.EF.Repositories;

internal sealed class CorporateOwnerRepository : ICorporateOwnerRepository
{
    private readonly WalletsDbContext _dbContext;

    public CorporateOwnerRepository(WalletsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<CorporateOwner> GetAsync(OwnerId id)
        => _dbContext.CorporateOwners.SingleOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(CorporateOwner owner)
    {
        await _dbContext.CorporateOwners.AddAsync(owner);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(CorporateOwner owner)
    {
        _dbContext.CorporateOwners.Update(owner);
        await _dbContext.SaveChangesAsync();
    }
}