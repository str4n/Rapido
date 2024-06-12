using System.Text.RegularExpressions;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.Entities.User;
using Rapido.Services.Users.Core.Exceptions;
using Rapido.Services.Users.Core.Repositories;

namespace Rapido.Services.Users.Core.Validators;

internal sealed class SignUpValidator : ISignUpValidator
{
    private static readonly Regex Regex = 
        new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
    
    private readonly IUserRepository _repository;

    public SignUpValidator(IUserRepository userRepository)
    {
        _repository = userRepository;
    }

    public async Task Validate(Email email, string password)
    {
        if (!Regex.IsMatch(password))
        {
            throw new InvalidPasswordException("Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.");
        }
        
        if (await _repository.GetAsync(email) is not null)
        {
            throw new UserAlreadyExistsException("Email is already taken.");
        }
    }
}