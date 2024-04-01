using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rapido.Framework.Postgres.Initializers;
using Rapido.Services.Users.Core.Entities.Role;

namespace Rapido.Services.Users.Core.EF;

internal sealed class UsersDataInitializer : IDataInitializer
{
    private readonly UsersDbContext _dbContext;
    private readonly ILogger<UsersDataInitializer> _logger;

    public UsersDataInitializer(UsersDbContext dbContext, ILogger<UsersDataInitializer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task InitAsync()
    {
        if (await _dbContext.Roles.AnyAsync())
        {
            return;
        }

        await AddRolesAsync();
        await _dbContext.SaveChangesAsync();
    }

    private async Task AddRolesAsync()
    {
        await _dbContext.Roles.AddAsync(new Role
        {
            Name = "admin",
        });
        await _dbContext.Roles.AddAsync(new Role
        {
            Name = "user",
        });

        _logger.LogInformation("Initialized roles.");
    }
}