using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
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

public class SignInEndpointTests : ApiTests<Program, UsersDbContext>
{
    [Fact]
    public async Task post_sign_up_should_create_account_and_sign_in_should_return_proper_jwt()
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

        var content = await signInResponse.Content.ReadFromJsonAsync<AuthDto>();

        content.Should().NotBeNull();
        content?.Token.Email.Should().Be(email);
        content?.Token.Role.Should().Be("user");
        content?.Token.AccessToken.Should().NotBeNullOrEmpty();
    }
    
    #region Arrange

    public SignInEndpointTests() : base(options => new UsersDbContext(options))
    {
    }
    
    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };

    #endregion
}