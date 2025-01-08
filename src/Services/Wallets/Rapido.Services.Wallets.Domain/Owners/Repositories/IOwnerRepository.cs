using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Domain.Owners.Repositories;

public interface IOwnerRepository
{
    public Task<Owner.Owner> GetAsync(string name, CancellationToken cancellationToken = default);
    public Task<Owner.Owner> GetAsync(OwnerId id, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Owner.Owner owner, CancellationToken cancellationToken = default);
}