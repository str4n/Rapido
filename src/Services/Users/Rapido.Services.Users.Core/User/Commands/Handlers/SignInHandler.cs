using Rapido.Framework.Auth.Authenticator;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Services.Users.Core.Shared.Exceptions;
using Rapido.Services.Users.Core.Shared.Storage;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.User.Repositories;
using Rapido.Services.Users.Core.User.Services;

namespace Rapido.Services.Users.Core.User.Commands.Handlers;

internal sealed class SignInHandler : ICommandHandler<SignIn>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IAuthenticator _authenticator;
    private readonly ITokenStorage _tokenStorage;

    public SignInHandler(IUserRepository userRepository, IPasswordManager passwordManager, IAuthenticator authenticator,
        ITokenStorage tokenStorage)
    {
        _userRepository = userRepository;
        _passwordManager = passwordManager;
        _authenticator = authenticator;
        _tokenStorage = tokenStorage;
    }
    
    public async Task HandleAsync(SignIn command)
    {
        var email = (Email)command.Email.ToLowerInvariant();
        var password = command.Password;

        var user = await _userRepository.GetAsync(email);

        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        if (!_passwordManager.Validate(password, user.Password))
        {
            throw new InvalidCredentialsException();
        }

        if (!user.IsActivated)
        {
            throw new UserNotActivatedException();
        }

        var jwt = _authenticator.CreateToken(user.Id, user.Role.Name, user.Email);
        _tokenStorage.Set(jwt);
    }
}