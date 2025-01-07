using Microsoft.EntityFrameworkCore;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.User.Repositories;

namespace Rapido.Services.Users.Core.Shared.EF.Repositories;

internal sealed class RoleRepository : IRoleRepository
{
    private readonly UsersDbContext _dbContext;

    public RoleRepository(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Role> GetAsync(string name, bool tracking = true)
        => tracking
            ? _dbContext.Roles.SingleOrDefaultAsync(x => x.Name == name)
            : _dbContext.Roles.AsNoTracking().SingleOrDefaultAsync(x => x.Name == name);

    public async Task<IReadOnlyCollection<Role>> GetAllAsync()
        => await _dbContext.Roles
            .AsNoTracking()
            .ToListAsync();
}