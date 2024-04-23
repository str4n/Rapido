using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Users.Core.Commands;
using Xunit;

namespace Rapido.Tests.Services.Users.E2E.Endpoints;

public class SignUpEndpointTests : IDisposable
{
    [Fact]
    public async Task post_sign_up_should_create_account_and_return_ok_status_code()
    {
        var command = new SignUp(Guid.NewGuid(), "test@gmail.com", "Testpasswd12!");

        var response = await _app.Client.PostAsJsonAsync("v1/sign-up", command);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task post_sign_up_with_existing_email_should_return_bad_request_status_code()
    {
        var command = new SignUp(Guid.NewGuid(), "test@gmail.com", "Testpasswd12!");

        var firstResponse = await _app.Client.PostAsJsonAsync("v1/sign-up", command);
        
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var secondResponse = await _app.Client.PostAsJsonAsync("v1/sign-up", command);

        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    
    public void Dispose()
    {
        _testDatabase.Dispose();
    }

    #region Arrange

    private readonly TestDatabase _testDatabase;
    private readonly TestApp<Program> _app;

    public SignUpEndpointTests()
    {
        _testDatabase = new TestDatabase();
        _app = new TestApp<Program>();
    }

    #endregion Arrange
}