using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Rapido.Framework.Auth;
using Rapido.Framework.Auth.Authenticator;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.Commands.Handlers;
using Rapido.Services.Users.Core.EF.Repositories;
using Rapido.Services.Users.Core.Entities.Role;
using Rapido.Services.Users.Core.Entities.User;
using Rapido.Services.Users.Core.Exceptions;
using Rapido.Services.Users.Core.Repositories;
using Rapido.Services.Users.Core.Services;
using Rapido.Services.Users.Core.Storage;
using Xunit;

namespace Rapido.Tests.Services.Users.Integration.Commands;

public class SignInHandlerTests : IDisposable
{
    private Task Act(SignIn command) => _handler.HandleAsync(command);
    
    [Fact]
    public async Task given_valid_sign_in_command_should_create_proper_jwt()
    {
        await _testDatabase.InitAsync();

        var email = Const.EmailInUse;
        var password = Const.ValidPassword;

        var command = new SignIn(email, password);

        await Act(command);

        var jwt = _tokenStorage.Get();

        jwt.Email.Should().Be(email.ToLowerInvariant());
        jwt.Role.Should().Be(Role.User);
        jwt.UserId.Should().NotBeEmpty();
        jwt.Token.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task given_sign_in_command_should_throw_exception()
    {
        await _testDatabase.InitAsync();

        var email = Const.EmailInUse;
        var password = "PasswOrd42@";

        var command = new SignIn(email, password);

        var act = async () => await Act(command);

        await act.Should().ThrowAsync<InvalidCredentialsException>();
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }
    
    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IAuthenticator _authenticator;
    private readonly IClock _clock;
    private readonly ITokenStorage _tokenStorage;

    private readonly SignInHandler _handler;

    public SignInHandlerTests()
    {
        _clock = new TestClock();
        _testDatabase = new TestDatabase();
        _userRepository = new UserRepository(_testDatabase.DbContext);
        _passwordManager = new PasswordManager(new PasswordHasher<User>());

        var options = new OptionsProvider().GetOptions<AuthOptions>("auth");
        _authenticator = new Authenticator(_clock, Options.Create(options));

        _tokenStorage = new TestTokenStorage();
        
        _handler = new SignInHandler(_userRepository, _passwordManager, _authenticator, _tokenStorage);
    }
    
    #endregion Arrange
}