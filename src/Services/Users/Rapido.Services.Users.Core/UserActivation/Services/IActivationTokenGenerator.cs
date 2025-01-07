namespace Rapido.Services.Users.Core.UserActivation.Services;

internal interface IActivationTokenGenerator
{
    Task<string> GenerateActivationToken();
}