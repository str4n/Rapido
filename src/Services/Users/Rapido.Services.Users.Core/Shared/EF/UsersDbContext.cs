using Microsoft.EntityFrameworkCore;
using Rapido.Services.Users.Core.PasswordRecovery.Domain;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.UserActivation.Domain;

namespace Rapido.Services.Users.Core.Shared.EF;

public sealed class UsersDbContext : DbContext
{
    public DbSet<User.Domain.User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserActivationToken> ActivationTokens { get; set; }
    public DbSet<PasswordRecoveryToken> RecoveryTokens { get; set; }
    
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}