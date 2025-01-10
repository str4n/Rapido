using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Auth;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Messages.Commands;
using Rapido.Services.Users.Core.EF;
using Rapido.Services.Users.Core.Shared.EF;
using Rapido.Services.Users.Core.User.Commands;
using Rapido.Services.Users.Core.User.DTO;
using Rapido.Services.Users.Core.UserActivation.Commands;
using Xunit;

namespace Rapido.Services.Users.Tests.E2E.Endpoints;

public class GetUserEndpointTests() : ApiTests<Program, UsersDbContext>(options => new UsersDbContext(options))
{
    [Fact]
    public async Task
        post_sign_up_should_create_account_and_sign_in_should_return_proper_jwt_get_should_return_account_info()
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

        var activationToken = await TestDbContext.ActivationTokens.FirstAsync();
        
        activationToken.Should().NotBeNull();

        var activationResponse = await Client.PutAsJsonAsync("/activate/", new ActivateUser(activationToken.Token));
        
        activationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var signInCommand = new SignIn(email, password);

        var signInResponse = await Client.PostAsJsonAsync("/sign-in", signInCommand);

        signInResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var jwt = (await signInResponse.Content.ReadFromJsonAsync<AuthDto>())?.Token.AccessToken;

        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var getResponse = await Client.GetAsync("/me");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await getResponse.Content.ReadFromJsonAsync<UserDto>();

        content.Should().NotBeNull();

        content?.Email.Should().Be(email);
        content?.Role.Should().Be("user");
    }


    #region Arrange

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };

    #endregion
}