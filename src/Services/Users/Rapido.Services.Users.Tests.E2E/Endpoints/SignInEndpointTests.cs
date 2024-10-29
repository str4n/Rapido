using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Rapido.Framework.Auth;
using Rapido.Framework.Testing;
using Rapido.Services.Users.Core.Commands;
using Xunit;

namespace Rapido.Services.Users.Tests.E2E.Endpoints;

public class SignInEndpointTests : IDisposable
{
    [Fact]
    public async Task post_sign_up_should_create_account_and_sign_in_should_return_proper_jwt()
    {
        await _testDatabase.InitAsync();
        
        var email = $"test{Guid.NewGuid():N}@gmail.com";
        var password = "TestPassword12!";
        var accountType = "Individual";

        var signUpCommand = new SignUp(Guid.Empty, email, password, accountType);
        
        var signUpResponse = await _app.Client.PostAsJsonAsync("/sign-up", signUpCommand);
        
        signUpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var signInCommand = new SignIn(email, password);

        var signInResponse = await _app.Client.PostAsJsonAsync("/sign-in", signInCommand);

        signInResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await signInResponse.Content.ReadFromJsonAsync<JsonWebToken>();

        content.Should().NotBeNull();
        content?.Email.Should().Be(email);
        content?.Role.Should().Be("user");
        content?.AccessToken.Should().NotBeNullOrEmpty();
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }

    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly TestApp<Program> _app;

    public SignInEndpointTests()
    {
        _testDatabase = new TestDatabase();
        _app = new TestApp<Program>();
    }

    #endregion Arrange
}