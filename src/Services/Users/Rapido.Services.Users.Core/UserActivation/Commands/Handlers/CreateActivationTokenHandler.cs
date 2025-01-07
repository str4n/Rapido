using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Commands;
using Rapido.Messages.Events;
using Rapido.Services.Users.Core.Shared.Exceptions;
using Rapido.Services.Users.Core.User.Repositories;
using Rapido.Services.Users.Core.UserActivation.Domain;
using Rapido.Services.Users.Core.UserActivation.Repositories;
using Rapido.Services.Users.Core.UserActivation.Services;

namespace Rapido.Services.Users.Core.UserActivation.Commands.Handlers;

internal sealed class CreateActivationTokenHandler(
    IActivationTokenRepository tokenRepository, 
    IActivationTokenGenerator tokenGenerator, 
    IUserRepository userRepository, 
    IMessageBroker messageBroker, 
    IClock clock) 
    : ICommandHandler<CreateActivationToken>
{
    private const int TokenLifetime = 24; // in hours
    
    public async Task HandleAsync(CreateActivationToken command)
    {
        var email = command.Email;
        var user = await userRepository.GetAsync(email, false);

        if (user is null)
        {
            throw new UserNotFoundException($"User with email: {email} was not found.");
        }

        if (user.IsActivated)
        {
            throw new CannotCreateActivationTokenException("User is already activated.");
        }
        
        if (user.IsDeleted)
        {
            return;
        }
        
        var token = await tokenGenerator.GenerateActivationToken();
        var now = clock.Now();
        var expires = now.AddHours(TokenLifetime);

        var activationToken = new UserActivationToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = token,
            CreatedAt = now,
            ExpiresOn = expires,
        };

        await tokenRepository.AddAsync(activationToken);
        await messageBroker.PublishAsync(new ActivationTokenCreated(user.Id, token));
    }
}