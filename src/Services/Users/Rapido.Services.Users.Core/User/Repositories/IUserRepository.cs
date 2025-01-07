namespace Rapido.Services.Users.Core.User.Repositories;

internal interface IUserRepository
{
    Task<User.Domain.User> GetAsync(Guid id, bool tracking = true);
    Task<User.Domain.User> GetAsync(string email, bool tracking = true);
    Task<bool> AnyAsync(string email);
    
    Task AddAsync(User.Domain.User user);
    Task UpdateAsync(User.Domain.User user);
}