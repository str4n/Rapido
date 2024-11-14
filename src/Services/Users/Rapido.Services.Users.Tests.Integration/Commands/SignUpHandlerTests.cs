using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.EF;
using Rapido.Services.Users.Core.Entities.Role;
using Rapido.Services.Users.Core.Entities.User;
using Rapido.Services.Users.Core.Exceptions;
using Rapido.Services.Users.Core.Services;
using Xunit;
using TestMessageBroker = Rapido.Framework.Testing.Abstractions.TestMessageBroker;

namespace Rapido.Services.Users.Tests.Integration.Commands;

public class SignUpHandlerTests : ApiTests<Program, UsersDbContext>
{
    private Task Act(SignUp command) => Dispatcher.DispatchAsync(command);

    [Fact]
    public async Task given_valid_sign_up_command_should_succeed()
    {
        var email = Const.ValidEmail;
        var password = Const.ValidPassword;
        var accountType = AccountType.Individual;
        var command = new SignUp(Guid.NewGuid(), email, password, accountType.ToString());

        await Act(command);

        var user = await TestDbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
        user.Should().NotBeNull();
        
        var passwordValid = new PasswordManager(new PasswordHasher<User>())
            .Validate(password, user.Password);
        
        passwordValid.Should().BeTrue();
    }

    [Fact]
    public async Task given_sign_up_command_with_already_taken_email_should_throw_exception()
    {
        var email = Const.EmailInUse;
        var password = Const.ValidPassword;
        var accountType = AccountType.Individual;
        var command = new SignUp(Guid.NewGuid(), email, password, accountType.ToString());

        var act = async () => await Act(command);

        await act.Should().ThrowAsync<UserAlreadyExistsException>();
    }

    [Theory]
    [InlineData("password")]
    [InlineData("Password")]
    [InlineData("password12")]
    [InlineData("password12!")]
    [InlineData("Password12")]
    [InlineData("Pa-s@ord12")]
    public async Task given_sign_up_command_with_invalid_password_syntax_should_throw_exception(string password)
    {
        var email = Const.ValidEmail;
        var accountType = AccountType.Individual;
        var command = new SignUp(Guid.NewGuid(), email, password, accountType.ToString());

        var act = async () => await Act(command);

        await act.Should().ThrowAsync<InvalidPasswordException>();
    }

    #region Arrange

    public SignUpHandlerTests() : base(options => new UsersDbContext(options))
    {
    }

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };

    protected override async Task SeedAsync()
    {
        var dbContext = TestDbContext;
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

    #endregion
}