using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Messages.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Users.Core.Entities.Role;
using Rapido.Services.Users.Core.Entities.User;
using Rapido.Services.Users.Core.Exceptions;
using Rapido.Services.Users.Core.Repositories;
using Rapido.Services.Users.Core.Services;
using Rapido.Services.Users.Core.Validators;

namespace Rapido.Services.Users.Core.Commands.Handlers;

internal sealed class SignUpHandler : ICommandHandler<SignUp>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IClock _clock;
    private readonly ISignUpValidator _validator;
    private readonly IPasswordManager _passwordManager;
    private readonly IMessageBroker _messageBroker;

    public SignUpHandler(IUserRepository userRepository, IRoleRepository roleRepository, IClock clock, 
        ISignUpValidator validator, IPasswordManager passwordManager, IMessageBroker messageBroker)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _clock = clock;
        _validator = validator;
        _passwordManager = passwordManager;
        _messageBroker = messageBroker;
    }
    
    public async Task HandleAsync(SignUp command)
    {
        var id = command.UserId;
        var email = (Email)command.Email?.ToLowerInvariant();
        var password = command.Password;
        
        await _validator.Validate(email, password);
        
        var securedPassword = _passwordManager.Secure(password);
        
        if (!Enum.TryParse(command.AccountType, out AccountType accountType))
        {
            throw new InvalidAccountTypeException();
        }

        var roleName = Role.Default;

        var role = await _roleRepository.GetAsync(Role.Default);

        if (role is null)
        {
            throw new RoleNotFoundException($"Role with name: {roleName} was not found.");
        }

        var user = new User
        {
            Id = id,
            Email = email,
            Password = securedPassword,
            Role = role,
            Type = accountType,
            IsActivated = false,
            IsDeleted = false,
            CreatedAt = _clock.Now()
        };

        await _userRepository.AddAsync(user);
        await _messageBroker.PublishAsync(new UserSignedUp(user.Id, user.Email, accountType.ToString(), user.CreatedAt));
    }
}