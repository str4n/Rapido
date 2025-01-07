using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Security.Hasher;
using Rapido.Messages.Events;
using Rapido.Services.Users.Core.PasswordRecovery.Domain;
using Rapido.Services.Users.Core.PasswordRecovery.Repositories;
using Rapido.Services.Users.Core.PasswordRecovery.Services;
using Rapido.Services.Users.Core.Shared.Exceptions;
using Rapido.Services.Users.Core.User.Repositories;

namespace Rapido.Services.Users.Core.PasswordRecovery.Commands.Handlers;

internal sealed class RequestRecoveryHandler(
    IRecoveryTokenRepository tokenRepository,
    IRecoveryTokenGenerator tokenGenerator,
    IUserRepository userRepository, 
    IHasher hasher,
    IMessageBroker messageBroker,
    IClock clock) 
    : ICommandHandler<CreateRecoveryToken>
{
    private const int TokenLifetime = 15; // in minutes
    
    public async Task HandleAsync(CreateRecoveryToken command)
    {
        var user = await userRepository.GetAsync(command.Email, false);
        
        if (user is null)
        {
            throw new UserNotFoundException($"User with email: '{command.Email}' was not found.");
        }

        if (user.IsDeleted)
        {
            return;
        }

        var token = await tokenGenerator.GenerateRecoveryToken();
        var now = clock.Now();
        var expires = now.AddMinutes(TokenLifetime);

        var recoveryToken = new PasswordRecoveryToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = hasher.Sha256(token),
            CreatedAt = now,
            ExpiresOn = expires,
        };

        await tokenRepository.AddAsync(recoveryToken);
        await messageBroker.PublishAsync(new RecoveryTokenCreated(user.Id, token));
    }
}