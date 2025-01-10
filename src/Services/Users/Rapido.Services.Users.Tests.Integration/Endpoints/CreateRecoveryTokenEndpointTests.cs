using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Users.Core.PasswordRecovery.Commands;
using Rapido.Services.Users.Core.Shared.EF;
using Rapido.Services.Users.Core.Shared.Storage;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.User.Services;
using Xunit;

namespace Rapido.Services.Users.Tests.Integration.Endpoints;

public class CreateRecoveryTokenEndpointTests() : ApiTests<Program, UsersDbContext>(options => new UsersDbContext(options))
{
    private Task<HttpResponseMessage> Act(CreateRecoveryToken command) 
        => Client.PutAsJsonAsync("/create-recovery-token", command);


    [Fact]
    public async Task given_valid_create_token_request_should_succeed()
    {
        var email = Const.SecondEmail;
        
        var response = await Act(new CreateRecoveryToken(email));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var recoveryToken = TestDbContext.RecoveryTokens.Single();

        recoveryToken.Should().NotBeNull();
        recoveryToken.CreatedAt.Should().Be(_clock.Now());
        recoveryToken.ExpiresOn.Should().Be(_clock.Now().AddMinutes(15));
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

        await dbContext.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            Email = Const.SecondEmail,
            Password = password,
            IsActivated = true,
            IsDeleted = false,
            CreatedAt = _clock.Now(),
            Role = role
        });

        await dbContext.SaveChangesAsync();
    }

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<ITokenStorage, TestTokenStorage>();
        s.AddScoped<IMessageBroker, TestMessageBroker>();
        s.AddScoped<IClock, TestClock>();
    };

    #endregion
}