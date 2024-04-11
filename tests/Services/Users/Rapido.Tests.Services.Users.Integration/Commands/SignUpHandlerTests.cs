using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.Commands.Handlers;
using Rapido.Services.Users.Core.EF.Repositories;
using Rapido.Services.Users.Core.Entities.User;
using Rapido.Services.Users.Core.Exceptions;
using Rapido.Services.Users.Core.Repositories;
using Rapido.Services.Users.Core.Services;
using Rapido.Services.Users.Core.Validators;
using Xunit;

namespace Rapido.Tests.Services.Users.Integration.Commands;

public class SignUpHandlerTests : IDisposable
{
    private Task Act(SignUp command) => _handler.HandleAsync(command);

    [Fact]
    public async Task given_valid_sign_up_command_should_succeed()
    {
        await _testDatabase.InitAsync();
        var email = Const.ValidEmail;
        var password = Const.ValidPassword;
        var command = new SignUp(Guid.NewGuid(), email, password);

        await Act(command);

        var user = await _userRepository.GetAsync(email);
        var passwordValid = _passwordManager.Validate(password, user.Password);

        user.Should().NotBeNull();
        passwordValid.Should().BeTrue();
    }

    [Fact]
    public async Task given_sign_up_command_with_already_taken_email_should_throw_exception()
    {
        await _testDatabase.InitAsync();
        var email = Const.EmailInUse;
        var password = Const.ValidPassword;
        var command = new SignUp(Guid.NewGuid(), email, password);

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
        await _testDatabase.InitAsync();

        var email = Const.ValidEmail;
        var command = new SignUp(Guid.NewGuid(), email, password);

        var act = async () => await Act(command);

        await act.Should().ThrowAsync<InvalidPasswordException>();
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;

    private readonly SignUpHandler _handler;

    public SignUpHandlerTests()
    {
        _testDatabase = new TestDatabase();
        var clock = new TestClock();
        _userRepository = new UserRepository(_testDatabase.DbContext);
        var roleRepository = new RoleRepository(_testDatabase.DbContext);
        var validator = new SignUpValidator(_userRepository);
        _passwordManager = new PasswordManager(new PasswordHasher<User>());
        var messageBroker = new TestMessageBroker();
        _handler = new SignUpHandler(_userRepository, roleRepository, clock, validator, _passwordManager, messageBroker);
    }
    
    #endregion Arrange
}