using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.EF;
using Xunit;

namespace Rapido.Services.Users.Tests.E2E.Endpoints;

public class SignUpEndpointTests : ApiTests<Program, UsersDbContext>
{
    [Fact]
    public async Task post_sign_up_should_create_account_and_return_ok_status_code()
    {
        var command = new SignUp(Guid.NewGuid(), $"test{Guid.NewGuid():N}@gmail.com", "Testpasswd12!", "Individual");

        var response = await Client.PostAsJsonAsync("/sign-up", command);
        
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task post_sign_up_with_existing_email_should_return_bad_request_status_code()
    {
        var command = new SignUp(Guid.NewGuid(), $"test{Guid.NewGuid():N}@gmail.com", "Testpasswd12!", "Individual");

        var firstResponse = await Client.PostAsJsonAsync("/sign-up", command);
        
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var secondResponse = await Client.PostAsJsonAsync("/sign-up", command);

        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    #region Arrange

    public SignUpEndpointTests() : base(options => new UsersDbContext(options))
    {
    }
    
    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };

    #endregion
}