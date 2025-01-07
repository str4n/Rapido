namespace Rapido.Services.Users.Core.User.Services;

internal interface IPasswordManager
{
    public string Secure(string password);
    public bool Validate(string password, string securedPassword);
}