using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Events;
using Rapido.Services.Users.Core.Exceptions;
using Rapido.Services.Users.Core.Repositories;

namespace Rapido.Services.Users.Core.Commands.Handlers;

internal sealed class ActivateUserHandler(IActivationTokenRepository tokenRepository, IUserRepository userRepository, 
    IClock clock, IMessageBroker messageBroker) 
    : ICommandHandler<ActivateUser>
{
    public async Task HandleAsync(ActivateUser command)
    {
        var activationToken = await tokenRepository.GetAsync(command.ActivationToken);

        if (activationToken is null)
        {
            throw new InvalidActivationTokenException("Activation token is invalid.");
        }

        var now = clock.Now();

        if (activationToken.ExpiresOn < now)
        {
            throw new InvalidActivationTokenException("Activation token expired.");
        }

        var userId = activationToken.UserId;
        var user = await userRepository.GetAsync(userId);

        if (user is null)
        {
            throw new UserNotFoundException($"User with id: {userId} was not found.");
        }

        if (user.IsActivated)
        {
            throw new UserAlreadyActivatedException(user.Email);
        }

        if (user.IsDeleted)
        {
            throw new UserDeletedException("User is deleted.");
        }
        
        user.IsActivated = true;
        user.ActivatedAt = now;
        
        await userRepository.UpdateAsync(user);
        await tokenRepository.DeleteAsync(activationToken);
        await messageBroker.PublishAsync(new UserActivated(userId));
    }
}