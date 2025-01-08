using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Domain.Owners.Repositories;

public interface ICorporateOwnerRepository
{
    public Task<CorporateOwner> GetAsync(OwnerId id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<CorporateOwner>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task AddAsync(CorporateOwner owner, CancellationToken cancellationToken = default);
    public Task UpdateAsync(CorporateOwner owner, CancellationToken cancellationToken = default);
}