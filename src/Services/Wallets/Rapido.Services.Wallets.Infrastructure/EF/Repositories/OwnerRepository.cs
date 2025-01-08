using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Infrastructure.EF.Repositories;

internal sealed class OwnerRepository(WalletsDbContext dbContext) : IOwnerRepository
{
    public Task<Owner> GetAsync(string name, CancellationToken cancellationToken = default)
        => dbContext.Owners.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);

    public Task<Owner> GetAsync(OwnerId id, CancellationToken cancellationToken = default)
        => dbContext.Owners.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task UpdateAsync(Owner owner, CancellationToken cancellationToken = default)
        => Task.FromResult(dbContext.Owners.Update(owner));
}