using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Security.Hasher;
using Rapido.Messages.Events;
using Rapido.Services.Users.Core.PasswordRecovery.Repositories;
using Rapido.Services.Users.Core.Shared.Exceptions;
using Rapido.Services.Users.Core.Shared.Validators;
using Rapido.Services.Users.Core.User.Repositories;
using Rapido.Services.Users.Core.User.Services;

namespace Rapido.Services.Users.Core.PasswordRecovery.Commands.Handlers;

internal sealed class RecoverPasswordHandler(
    IRecoveryTokenRepository tokenRepository, 
    IUserRepository userRepository, 
    IClock clock,
    IHasher hasher,
    IPasswordValidator passwordValidator,
    IPasswordManager passwordManager,
    IMessageBroker messageBroker) 
    : ICommandHandler<RecoverPassword>
{
    public async Task HandleAsync(RecoverPassword command)
    {
        var newPassword = command.NewPassword;
        
        passwordValidator.Validate(newPassword);
        
        var tokenHash = hasher.Sha256(command.RecoveryToken);
        
        var recoveryToken = await tokenRepository.GetAsync(tokenHash);

        if (recoveryToken is null)
        {
            throw new InvalidRecoveryTokenException("Recovery token is invalid.");
        }

        var now = clock.Now();

        if (recoveryToken.ExpiresOn < now)
        {
            throw new InvalidRecoveryTokenException("Recovery token expired.");
        }

        var userId = recoveryToken.UserId;
        var user = await userRepository.GetAsync(userId);

        if (user is null)
        {
            throw new UserNotFoundException("User was not found.");
        }

        if (!string.Equals(user.Email.Value, command.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new InvalidRecoveryTokenException("Recovery token is invalid.");
        }

        var securedPassword = passwordManager.Secure(newPassword);

        user.Password = securedPassword;

        await userRepository.UpdateAsync(user);
        await tokenRepository.DeleteAsync(recoveryToken);
        await messageBroker.PublishAsync(new PasswordRecovered(userId));
    }
}