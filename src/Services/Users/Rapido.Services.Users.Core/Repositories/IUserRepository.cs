using Rapido.Services.Users.Core.Entities.User;

namespace Rapido.Services.Users.Core.Repositories;

internal interface IUserRepository
{
    Task<User> GetAsync(Guid id, bool tracking = true);
    Task<User> GetAsync(string email, bool tracking = true);
    Task<bool> AnyAsync(string email);
    
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}