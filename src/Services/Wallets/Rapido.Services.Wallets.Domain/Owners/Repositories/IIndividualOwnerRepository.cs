using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Domain.Owners.Repositories;

public interface IIndividualOwnerRepository
{
    public Task<IndividualOwner> GetAsync(OwnerId id, CancellationToken cancellationToken = default);
    public Task<IEnumerable<IndividualOwner>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task AddAsync(IndividualOwner owner, CancellationToken cancellationToken = default);
    public Task UpdateAsync(IndividualOwner owner, CancellationToken cancellationToken = default);
    public Task DeleteAsync(IndividualOwner owner, CancellationToken cancellationToken = default);
}