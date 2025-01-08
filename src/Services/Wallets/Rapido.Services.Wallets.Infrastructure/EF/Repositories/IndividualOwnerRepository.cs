using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Infrastructure.EF.Repositories;

internal sealed class IndividualOwnerRepository(WalletsDbContext dbContext) : IIndividualOwnerRepository
{
    public Task<IndividualOwner> GetAsync(OwnerId id, CancellationToken cancellationToken = default)
        => dbContext.IndividualOwners.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IEnumerable<IndividualOwner>> GetAllAsync(CancellationToken cancellationToken = default)
        => await dbContext.IndividualOwners.ToListAsync(cancellationToken);

    public async Task AddAsync(IndividualOwner owner, CancellationToken cancellationToken = default)
        => await dbContext.IndividualOwners.AddAsync(owner, cancellationToken);

    public Task UpdateAsync(IndividualOwner owner, CancellationToken cancellationToken = default)
        => Task.FromResult(dbContext.IndividualOwners.Update(owner));

    public Task DeleteAsync(IndividualOwner owner, CancellationToken cancellationToken = default)
        => Task.FromResult(dbContext.IndividualOwners.Remove(owner));
}