using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Infrastructure.EF.Repositories;

internal sealed class IndividualOwnerRepository : IIndividualOwnerRepository
{
    private readonly WalletsDbContext _dbContext;

    public IndividualOwnerRepository(WalletsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IndividualOwner> GetAsync(OwnerId id)
        => _dbContext.IndividualOwners.SingleOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<IndividualOwner>> GetAllAsync()
        => await _dbContext.IndividualOwners.ToListAsync();

    public async Task AddAsync(IndividualOwner owner)
    {
        await _dbContext.IndividualOwners.AddAsync(owner);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(IndividualOwner owner)
    {
        _dbContext.IndividualOwners.Update(owner);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(IndividualOwner owner)
    {
        _dbContext.IndividualOwners.Remove(owner);
        await _dbContext.SaveChangesAsync();
    }
}