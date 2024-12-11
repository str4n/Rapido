using Microsoft.EntityFrameworkCore;
using Rapido.Services.Users.Core.Entities.ActivationToken;
using Rapido.Services.Users.Core.Entities.Role;
using Rapido.Services.Users.Core.Entities.User;

namespace Rapido.Services.Users.Core.EF;

public sealed class UsersDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserActivationToken> ActivationTokens { get; set; }
    
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}