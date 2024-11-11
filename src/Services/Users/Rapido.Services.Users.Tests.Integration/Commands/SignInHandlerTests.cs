using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.EF;
using Rapido.Services.Users.Core.Entities.Role;
using Rapido.Services.Users.Core.Entities.User;
using Rapido.Services.Users.Core.Exceptions;
using Rapido.Services.Users.Core.Services;
using Rapido.Services.Users.Core.Storage;
using Xunit;

namespace Rapido.Services.Users.Tests.Integration.Commands;

public class SignInHandlerTests : ApiTests<Program, UsersDbContext>
{
    private Task Act(SignIn command) => Dispatcher.DispatchAsync(command);
    
    [Fact]
    public async Task given_valid_sign_in_command_should_create_proper_jwt()
    {
        var tokenStorage = Scope.ServiceProvider.GetRequiredService<ITokenStorage>();
        var email = Const.EmailInUse;
        var password = Const.ValidPassword;

        var command = new SignIn(email, password);

        await Act(command);

        var jwt = tokenStorage.Get();

        jwt.Email.Should().Be(email.ToLowerInvariant());
        jwt.Role.Should().Be(Role.User);
        jwt.UserId.Should().NotBeEmpty();
        jwt.AccessToken.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task given_sign_in_command_should_throw_exception()
    {
        var email = Const.EmailInUse;
        var password = "PasswOrd42@";

        var command = new SignIn(email, password);

        var act = async () => await Act(command);

        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }

    #region Arrange

    protected override async Task SeedAsync()
    {
        var dbContext = GetDbContext();
        await dbContext.Database.MigrateAsync();
        
        var clock = new TestClock();
        
        var role = new Role
        {
            Name = Role.User
        };

        await dbContext.Roles.AddAsync(role);

        var password = new PasswordManager(new PasswordHasher<User>()).Secure(Const.ValidPassword);

        await dbContext.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            Email = Const.EmailInUse,
            Password = password,
            State = UserState.Active,
            CreatedAt = clock.Now(),
            Role = role
        });

        await dbContext.SaveChangesAsync();
    }

    public SignInHandlerTests() : base(options => new UsersDbContext(options))
    {
    }

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<ITokenStorage, TestTokenStorage>();
    };

    #endregion
}