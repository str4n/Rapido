using Microsoft.EntityFrameworkCore;
using Rapido.Services.Users.Core.Entities.User;
using Rapido.Services.Users.Core.Repositories;

namespace Rapido.Services.Users.Core.EF.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly UsersDbContext _dbContext;

    public UserRepository(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User> GetAsync(Guid id)
        => _dbContext.Users
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == id);

    public Task<User> GetAsync(string email)
        => _dbContext.Users
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.Email == email);

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}