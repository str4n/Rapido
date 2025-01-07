using Microsoft.AspNetCore.Identity;

namespace Rapido.Services.Users.Core.User.Services;

internal sealed class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher<User.Domain.User>_passwordHasher;

    public PasswordManager(IPasswordHasher<User.Domain.User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string Secure(string password) => _passwordHasher.HashPassword(default!, password);

    public bool Validate(string password, string securedPassword)
        => _passwordHasher.VerifyHashedPassword(default!, securedPassword, password) 
            is PasswordVerificationResult.Success;
}