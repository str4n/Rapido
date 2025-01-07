namespace Rapido.Services.Users.Core.PasswordRecovery.Services;

public interface IRecoveryTokenGenerator
{
    Task<string> GenerateRecoveryToken();
}