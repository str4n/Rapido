using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Infrastructure.EF.Repositories;

internal sealed class OwnerRepository : IOwnerRepository
{
    private readonly WalletsDbContext _dbContext;

    public OwnerRepository(WalletsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Owner> GetAsync(string name)
        => _dbContext.Owners.SingleOrDefaultAsync(x => x.Name == name);
}