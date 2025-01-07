using Rapido.Services.Users.Core.User.Domain;

namespace Rapido.Services.Users.Core.User.Repositories;

internal interface IRoleRepository
{
    Task<Role> GetAsync(string name, bool tracking = true);
    Task<IReadOnlyCollection<Role>> GetAllAsync();
}