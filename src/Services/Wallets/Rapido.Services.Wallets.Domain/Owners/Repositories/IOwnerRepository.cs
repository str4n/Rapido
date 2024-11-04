namespace Rapido.Services.Wallets.Domain.Owners.Repositories;

public interface IOwnerRepository
{
    public Task<Owner.Owner> GetAsync(string name);
}