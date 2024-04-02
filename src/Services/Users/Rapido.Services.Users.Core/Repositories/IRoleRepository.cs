using Rapido.Services.Users.Core.Entities.Role;

namespace Rapido.Services.Users.Core.Repositories;

internal interface IRoleRepository
{
    Task<Role> GetAsync(string name, bool tracking = true);
    Task<IReadOnlyCollection<Role>> GetAllAsync();
}