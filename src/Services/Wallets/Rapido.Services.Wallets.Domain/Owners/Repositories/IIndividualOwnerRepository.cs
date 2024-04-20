using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Domain.Owners.Repositories;

public interface IIndividualOwnerRepository
{
    public Task<IndividualOwner> GetAsync(OwnerId id);
    public Task<IEnumerable<IndividualOwner>> GetAllAsync();
    public Task AddAsync(IndividualOwner owner);
    public Task UpdateAsync(IndividualOwner owner);
    public Task DeleteAsync(IndividualOwner owner);
}