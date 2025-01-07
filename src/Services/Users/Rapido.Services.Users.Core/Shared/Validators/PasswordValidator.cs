using System.Text.RegularExpressions;
using Rapido.Services.Users.Core.Shared.Exceptions;

namespace Rapido.Services.Users.Core.Shared.Validators;

internal sealed class PasswordValidator : IPasswordValidator
{
    private static readonly Regex Regex = 
        new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
    
    public void Validate(string password)
    {
        if (!Regex.IsMatch(password))
        {
            throw new InvalidPasswordException("Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character.");
        }
    }
}