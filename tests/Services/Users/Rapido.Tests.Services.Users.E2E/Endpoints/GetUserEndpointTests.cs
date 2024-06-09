using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Rapido.Framework.Auth;
using Rapido.Framework.Testing;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.DTO;
using Xunit;

namespace Rapido.Tests.Services.Users.E2E.Endpoints;

public class GetUserEndpointTests : IDisposable
{
    [Fact]
    public async Task
        post_sign_up_should_create_account_and_sign_in_should_return_proper_jwt_get_should_return_account_info()
    {
        var email = "test312@gmail.com";
        var password = "TestPassword12!";
        var accountType = "Individual";

        var signUpCommand = new SignUp(Guid.Empty, email, password, accountType);
        
        var signUpResponse = await _app.Client.PostAsJsonAsync("/sign-up", signUpCommand);
        
        signUpResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var signInCommand = new SignIn(email, password);

        var signInResponse = await _app.Client.PostAsJsonAsync("/sign-in", signInCommand);

        signInResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var jwt = (await signInResponse.Content.ReadFromJsonAsync<JsonWebToken>())?.Token;

        _app.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var getResponse = await _app.Client.GetAsync("/me");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await getResponse.Content.ReadFromJsonAsync<UserDto>();

        content.Should().NotBeNull();

        content?.Email.Should().Be(email);
        content?.Role.Should().Be("user");
    }
    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }

    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly TestApp<Program> _app;

    public GetUserEndpointTests()
    {
        _testDatabase = new TestDatabase();
        _app = new TestApp<Program>();
    }

    #endregion Arrange
}