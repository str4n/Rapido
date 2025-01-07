namespace Rapido.Services.Users.Core.Shared.Validators;

internal interface IPasswordValidator
{
    void Validate(string password);
}