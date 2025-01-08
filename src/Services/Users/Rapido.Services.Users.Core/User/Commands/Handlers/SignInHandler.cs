using Rapido.Framework.Auth.Authenticator;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Services.Users.Core.Shared.Exceptions;
using Rapido.Services.Users.Core.Shared.Storage;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.User.Repositories;
using Rapido.Services.Users.Core.User.Services;

namespace Rapido.Services.Users.Core.User.Commands.Handlers;

internal sealed class SignInHandler(
    IUserRepository userRepository,
    IPasswordManager passwordManager,
    IAuthenticator authenticator,
    ITokenStorage tokenStorage)
    : ICommandHandler<SignIn>
{
    public async Task HandleAsync(SignIn command, CancellationToken cancellationToken = default)
    {
        var email = (Email)command.Email.ToLowerInvariant();
        var password = command.Password;

        var user = await userRepository.GetAsync(email);

        if (user is null)
        {
            throw new InvalidCredentialsException();
        }

        if (!passwordManager.Validate(password, user.Password))
        {
            throw new InvalidCredentialsException();
        }

        if (!user.IsActivated)
        {
            throw new UserNotActivatedException();
        }

        var jwt = authenticator.CreateToken(user.Id, user.Role.Name, user.Email);
        tokenStorage.Set(jwt);
    }
}