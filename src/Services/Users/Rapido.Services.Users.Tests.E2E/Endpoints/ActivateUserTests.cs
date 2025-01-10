using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Messages.Commands;
using Rapido.Services.Users.Core.Shared.EF;
using Rapido.Services.Users.Core.User.Commands;
using Rapido.Services.Users.Core.UserActivation.Commands;
using Xunit;

namespace Rapido.Services.Users.Tests.E2E.Endpoints;

public class ActivateUserTests() : ApiTests<Program, UsersDbContext>(options => new UsersDbContext(options))
{
    [Fact]
    public async Task account_creation_and_activation_should_succeed()
    {
        var email = $"test{Guid.NewGuid():N}@gmail.com";
        var password = "TestPassword12!";
        var accountType = "Individual";

        var signUpCommand = new SignUp(Guid.Empty, email, password, accountType);
        
        var signUpResponse = await Client.PostAsJsonAsync("/sign-up", signUpCommand);
        
        signUpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Saga creates token and sends it via email
        
        var createActivationTokenCommand = new CreateActivationToken(email);

        var createActivationTokenResponse =
            await Client.PutAsJsonAsync("create-activation-token", createActivationTokenCommand);

        createActivationTokenResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var activationToken = await TestDbContext.ActivationTokens.FirstAsync();
        
        activationToken.Should().NotBeNull();

        var activationResponse = await Client.PutAsJsonAsync("/activate/", new ActivateUser(activationToken.Token));
        
        activationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await TestDbContext.Users.FirstAsync();

        user.Should().NotBeNull();
        user.IsActivated.Should().BeTrue();
        user.ActivatedAt.Should().Be(_clock.Now());
    }
    
    
    #region Arrange

    private readonly IClock _clock = new TestClock();

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
        s.AddSingleton<IClock, TestClock>();
    };

    #endregion
}