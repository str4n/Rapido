using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Domain.Owners.Repositories;

public interface IOwnerRepository
{
    public Task<Owner.Owner> GetAsync(string name);
    public Task<Owner.Owner> GetAsync(OwnerId id);
    public Task UpdateAsync(Owner.Owner owner);
}