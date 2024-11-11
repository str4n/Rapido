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
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.DTO;
using Rapido.Services.Users.Core.EF;
using Xunit;

namespace Rapido.Services.Users.Tests.E2E.Endpoints;

public class GetUserEndpointTests : ApiTests<Program, UsersDbContext>
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

    public GetUserEndpointTests() : base(options => new UsersDbContext(options))
    {
    }
    
    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };

    #endregion
}