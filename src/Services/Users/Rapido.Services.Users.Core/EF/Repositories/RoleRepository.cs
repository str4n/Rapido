using Microsoft.EntityFrameworkCore;
using Rapido.Services.Users.Core.Entities.Role;
using Rapido.Services.Users.Core.Repositories;

namespace Rapido.Services.Users.Core.EF.Repositories;

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