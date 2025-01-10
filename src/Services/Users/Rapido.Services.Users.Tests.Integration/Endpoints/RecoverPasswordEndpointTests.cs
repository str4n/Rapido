using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Users.Core.PasswordRecovery.Commands;
using Rapido.Services.Users.Core.PasswordRecovery.Domain;
using Rapido.Services.Users.Core.Shared.EF;
using Rapido.Services.Users.Core.Shared.Storage;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.User.Services;
using Xunit;

namespace Rapido.Services.Users.Tests.Integration.Endpoints;

public class RecoverPasswordEndpointTests() : ApiTests<Program, UsersDbContext>(options => new UsersDbContext(options))
{
    private Task<HttpResponseMessage> Act(RecoverPassword command) 
        => Client.PutAsJsonAsync("/recover-password", command);
    
    [Fact]
    public async Task valid_recover_password_request_should_change_password()
    {
        var email = Const.SecondEmail;
        var recoveryToken = Const.RecoveryToken;
        var newPassword = "Test122!";

        var response = await Act(new RecoverPassword(email, recoveryToken, newPassword));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task recover_password_request_with_invalid_email_should_return_bad_request_status_code()
    {
        var email = "invalid@gmail.com";
        var recoveryToken = Const.RecoveryToken;
        var newPassword = "Test122!";

        var response = await Act(new RecoverPassword(email, recoveryToken, newPassword));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task recover_password_request_with_invalid_token_should_return_bad_request_status_code()
    {
        var email = Const.SecondEmail;
        var recoveryToken = "1NV4L1D";
        var newPassword = "Test122!";

        var response = await Act(new RecoverPassword(email, recoveryToken, newPassword));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    
    #region Arrange

    private readonly IClock _clock = new TestClock();

    protected override async Task SeedAsync()
    {
        var dbContext = TestDbContext;
        await dbContext.Database.MigrateAsync(); ;
        
        var role = new Role
        {
            Name = Role.User
        };

        await dbContext.Roles.AddAsync(role);

        var password = new PasswordManager(new PasswordHasher<User>()).Secure(Const.ValidPassword);

        var userId = Guid.NewGuid();

        await dbContext.Users.AddAsync(new User
        {
            Id = userId,
            Email = Const.SecondEmail,
            Password = password,
            IsActivated = true,
            IsDeleted = false,
            CreatedAt = _clock.Now(),
            Role = role
        });

        await dbContext.RecoveryTokens.AddAsync(new PasswordRecoveryToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = Sha256(Const.RecoveryToken),
            CreatedAt = _clock.Now(),
            ExpiresOn = _clock.Now().AddMinutes(15)
        });

        await dbContext.SaveChangesAsync();

        string Sha256(string value)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));

            var sb = new StringBuilder();
        
            foreach (var @byte in bytes)
            {
                sb.Append(@byte.ToString("X"));
            }

            return sb.ToString();
        }
    }

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<ITokenStorage, TestTokenStorage>();
        s.AddScoped<IMessageBroker, TestMessageBroker>();
        s.AddScoped<IClock, TestClock>();
    };

    #endregion
}