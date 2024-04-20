using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Domain.Owners.Repositories;

public interface ICorporateOwnerRepository
{
    public Task<CorporateOwner> GetAsync(OwnerId id);
    public Task AddAsync(CorporateOwner owner);
    public Task UpdateAsync(CorporateOwner owner);
}