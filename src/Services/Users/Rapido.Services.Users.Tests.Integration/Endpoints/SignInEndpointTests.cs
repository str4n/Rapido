using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Users.Core.EF;
using Rapido.Services.Users.Core.Shared.EF;
using Rapido.Services.Users.Core.Shared.Storage;
using Rapido.Services.Users.Core.User.Commands;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.User.DTO;
using Rapido.Services.Users.Core.User.Services;
using Xunit;

namespace Rapido.Services.Users.Tests.Integration.Endpoints;

public class SignInEndpointTests() : ApiTests<Program, UsersDbContext>(options => new UsersDbContext(options))
{
    private Task<HttpResponseMessage> Act(SignIn command) => Client.PostAsJsonAsync("/sign-in", command);
    
    [Fact]
    public async Task given_valid_sign_in_request_should_create_proper_jwt()
    {
        var email = Const.EmailInUse;
        var password = Const.ValidPassword;

        var command = new SignIn(email, password);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jwt = (await response.Content.ReadFromJsonAsync<AuthDto>()).Token;

        jwt.Email.Should().Be(email.ToLowerInvariant());
        jwt.Role.Should().Be(Role.User);
        jwt.UserId.Should().NotBeEmpty();
        jwt.AccessToken.Should().NotBeNullOrWhiteSpace();
    }
    
    [Fact]
    public async Task given_sign_in_request_with_invalid_credentials_should_return_bad_request_status_code()
    {
        var email = Const.EmailInUse;
        var password = "PasswOrd42@";

        var command = new SignIn(email, password);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #region Arrange

    protected override async Task SeedAsync()
    {
        var dbContext = TestDbContext;
        await dbContext.Database.MigrateAsync();
        
        var clock = new TestClock();
        
        var role = new Role
        {
            Name = Role.User
        };

        await dbContext.Roles.AddAsync(role);

        var password = new PasswordManager(new PasswordHasher<User>()).Secure(Const.ValidPassword);

        await dbContext.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            Email = Const.EmailInUse,
            Password = password,
            IsActivated = true,
            IsDeleted = false,
            CreatedAt = clock.Now(),
            Role = role
        });

        await dbContext.SaveChangesAsync();
    }

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<ITokenStorage, TestTokenStorage>();
    };

    #endregion
}