using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Users.Core.EF;
using Rapido.Services.Users.Core.Entities.Role;
using Rapido.Services.Users.Core.Entities.User;
using Rapido.Services.Users.Core.Services;

namespace Rapido.Services.Users.Tests.Integration;

internal sealed class TestDatabase : IDisposable
{
    private readonly IClock _clock;
    public UsersDbContext DbContext { get; }

    public TestDatabase()
    {
        var connectionString = $"Host=localhost;Database=rapido-users-tests-{Guid.NewGuid():N};Username=postgres;Password=Admin12!";
        DbContext = new UsersDbContext(new DbContextOptionsBuilder<UsersDbContext>()
            .UseNpgsql(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging().Options);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        _clock = new TestClock();
    }

    public async Task InitAsync()
    {
        await DbContext.Database.MigrateAsync();

        var role = new Role
        {
            Name = Role.User
        };

        await DbContext.Roles.AddAsync(role);

        var password = new PasswordManager(new PasswordHasher<User>()).Secure(Const.ValidPassword);

        await DbContext.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            Email = Const.EmailInUse,
            Password = password,
            State = UserState.Active,
            CreatedAt = _clock.Now(),
            Role = role
        });

        await DbContext.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}