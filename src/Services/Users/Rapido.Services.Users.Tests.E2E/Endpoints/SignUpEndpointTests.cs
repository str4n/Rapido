﻿using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Testing;
using Rapido.Services.Users.Core.Commands;
using Xunit;

namespace Rapido.Services.Users.Tests.E2E.Endpoints;

public class SignUpEndpointTests : IDisposable
{
    [Fact]
    public async Task post_sign_up_should_create_account_and_return_ok_status_code()
    {
        await _testDatabase.InitAsync();
        
        var command = new SignUp(Guid.NewGuid(), $"test{Guid.NewGuid():N}@gmail.com", "Testpasswd12!", "Individual");

        var response = await _app.Client.PostAsJsonAsync("/sign-up", command);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task post_sign_up_with_existing_email_should_return_bad_request_status_code()
    {
        await _testDatabase.InitAsync();
        
        var command = new SignUp(Guid.NewGuid(), $"test{Guid.NewGuid():N}@gmail.com", "Testpasswd12!", "Individual");

        var firstResponse = await _app.Client.PostAsJsonAsync("/sign-up", command);
        
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var secondResponse = await _app.Client.PostAsJsonAsync("/sign-up", command);

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