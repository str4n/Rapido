using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Messages.Commands;
using Rapido.Messages.Events;
using Rapido.Services.Users.Core.PasswordRecovery.Commands;
using Rapido.Services.Users.Core.Shared.EF;
using Rapido.Services.Users.Core.User.Commands;
using Rapido.Services.Users.Core.UserActivation.Commands;
using Xunit;

namespace Rapido.Services.Users.Tests.E2E.Endpoints;

public class RecoverPasswordTests() : ApiTests<Program, UsersDbContext>(options => new UsersDbContext(options))
{
    [Fact]
    public async Task account_creation_and_password_recovery_should_succeed()
    {
        var email = $"test{Guid.NewGuid():N}@gmail.com";
        var password = "TestPassword12!";
        var accountType = "Individual";

        var signUpCommand = new SignUp(Guid.Empty, email, password, accountType);
        
        var signUpResponse = await Client.PostAsJsonAsync("/sign-up", signUpCommand);
        
        signUpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Saga should take care of token creation
        
        var createActivationTokenCommand = new CreateActivationToken(email);

        var createActivationTokenResponse =
            await Client.PutAsJsonAsync("create-activation-token", createActivationTokenCommand);

        createActivationTokenResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var activationToken = TestDbContext.ActivationTokens.First();
        
        activationToken.Should().NotBeNull();

        var activationResponse = await Client.PutAsJsonAsync("/activate/", new ActivateUser(activationToken.Token));
        
        activationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var signInCommand = new SignIn(email, password);

        var signInResponse = await Client.PostAsJsonAsync("/sign-in", signInCommand);

        signInResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createRecoveryTokenCommand = new CreateRecoveryToken(email);

        var createRecoveryTokenResponse =
            await Client.PutAsJsonAsync("create-recovery-token", createRecoveryTokenCommand);

        createRecoveryTokenResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var newPassword = "Test122!";
        var recoveryToken = TestMessageBroker.Messages.OfType<RecoveryTokenCreated>().Last();

        var recoverPasswordCommand = new RecoverPassword(email, recoveryToken.Token, newPassword);

        var recoverPasswordResponse = await Client.PutAsJsonAsync("recover-password", recoverPasswordCommand);

        recoverPasswordResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var secondSignInCommand = new SignIn(email, newPassword);

        var secondSignInResponse = await Client.PostAsJsonAsync("/sign-in", secondSignInCommand);

        secondSignInResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    
    #region Arrange

    private static readonly TestMessageBroker TestMessageBroker = new();

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker>(_ => TestMessageBroker);
    };

    #endregion
}