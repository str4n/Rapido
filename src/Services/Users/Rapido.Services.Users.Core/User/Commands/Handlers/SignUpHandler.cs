using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Events;
using Rapido.Services.Users.Core.Shared.Exceptions;
using Rapido.Services.Users.Core.Shared.Validators;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.User.Repositories;
using Rapido.Services.Users.Core.User.Services;

namespace Rapido.Services.Users.Core.User.Commands.Handlers;

internal sealed class SignUpHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IClock clock,
    IPasswordValidator passwordValidator,
    IPasswordManager passwordManager,
    IMessageBroker messageBroker)
    : ICommandHandler<SignUp>
{
    public async Task HandleAsync(SignUp command, CancellationToken cancellationToken = default)
    {
        var id = command.UserId;
        var email = (Email)command.Email?.ToLowerInvariant();
        var password = command.Password;
        
        if (await userRepository.AnyAsync(email))
        {
            throw new UserAlreadyExistsException("Email is already taken.");
        }
        
        passwordValidator.Validate(password);
        
        var securedPassword = passwordManager.Secure(password);
        
        if (!Enum.TryParse(command.AccountType, out AccountType accountType))
        {
            throw new InvalidAccountTypeException();
        }

        var roleName = Role.Default;

        var role = await roleRepository.GetAsync(Role.Default);

        if (role is null)
        {
            throw new RoleNotFoundException($"Role with name: {roleName} was not found.");
        }

        var user = new User.Domain.User
        {
            Id = id,
            Email = email,
            Password = securedPassword,
            Role = role,
            Type = accountType,
            IsActivated = false,
            IsDeleted = false,
            CreatedAt = clock.Now()
        };

        await userRepository.AddAsync(user);
        await messageBroker.PublishAsync(new UserSignedUp(user.Id, user.Email, accountType.ToString(), user.CreatedAt));
    }
}