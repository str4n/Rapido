using Microsoft.EntityFrameworkCore;
using Rapido.Services.Users.Core.User.Repositories;

namespace Rapido.Services.Users.Core.Shared.EF.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly UsersDbContext _dbContext;

    public UserRepository(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User.Domain.User> GetAsync(Guid id, bool tracking = true)
        => tracking
            ? _dbContext.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == id)
            : _dbContext.Users
                .AsNoTracking()
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == id);

    public Task<User.Domain.User> GetAsync(string email, bool tracking = true)
        => tracking
            ? _dbContext.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Email == email)
            : _dbContext.Users
                .AsNoTracking()
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Email == email);

    public Task<bool> AnyAsync(string email)
        => _dbContext.Users.AnyAsync(x => x.Email == email);

    public async Task AddAsync(User.Domain.User user)
        => await _dbContext.Users.AddAsync(user);

    public Task UpdateAsync(User.Domain.User user)
        => Task.FromResult(_dbContext.Users.Update(user));
}