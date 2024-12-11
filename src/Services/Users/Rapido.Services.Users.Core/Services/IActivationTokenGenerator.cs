namespace Rapido.Services.Users.Core.Services;

internal interface IActivationTokenGenerator
{
    Task<string> GenerateActivationToken();
}