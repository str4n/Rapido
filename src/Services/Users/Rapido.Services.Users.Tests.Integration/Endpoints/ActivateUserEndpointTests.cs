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
using Rapido.Services.Users.Core.Shared.EF;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.User.Services;
using Rapido.Services.Users.Core.UserActivation.Commands;
using Rapido.Services.Users.Core.UserActivation.Domain;
using Xunit;

namespace Rapido.Services.Users.Tests.Integration.Endpoints;

public class ActivateUserEndpointTests() : ApiTests<Program, UsersDbContext>(options => new UsersDbContext(options))
{
    private Task<HttpResponseMessage> Act(ActivateUser command) 
        => Client.PutAsJsonAsync("/activate", command);

    [Fact]
    public async Task given_valid_activate_user_request_should_succeed()
    {
        var response = await Act(new ActivateUser(Const.SecondActivationToken));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await TestDbContext.Users.SingleOrDefaultAsync(x => x.Email == Const.SecondEmail);

        user.IsActivated.Should().BeTrue();
        user.ActivatedAt.Should().Be(_clock.Now());
    }
    
    [Fact]
    public async Task already_activated_user_request_should_return_bad_request_status_code()
    {
        var response = await Act(new ActivateUser(Const.FirstActivationToken));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task invalid_activation_token_request_should_return_bad_request_status_code()
    {
        var response = await Act(new ActivateUser("INV4L1D"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    
    #region Arrange

    private readonly IClock _clock = new TestClock();

    protected override async Task SeedAsync()
    {
        var dbContext = TestDbContext;
        await dbContext.Database.MigrateAsync();
        
        var role = new Role
        {
            Name = Role.User
        };

        await dbContext.Roles.AddAsync(role);

        var password = new PasswordManager(new PasswordHasher<User>()).Secure(Const.ValidPassword);

        var firstUserId = Guid.NewGuid();
        var secondUserId = Guid.NewGuid();

        var users = new List<User>()
        {
            new()
            {
                Id = firstUserId,
                Email = Const.FirstEmail,
                Password = password,
                IsActivated = true,
                IsDeleted = false,
                CreatedAt = _clock.Now(),
                Role = role
            },
            new()
            {
                Id = secondUserId,
                Email = Const.SecondEmail,
                Password = password,
                IsActivated = false,
                IsDeleted = false,
                CreatedAt = _clock.Now(),
                Role = role
            }
        };

        var activationTokens = new List<UserActivationToken>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserId = firstUserId,
                CreatedAt = _clock.Now(),
                ExpiresOn = _clock.Now().AddMinutes(15),
                Token = Const.FirstActivationToken
            },
            new()
            {
                Id = Guid.NewGuid(),
                UserId = secondUserId,
                CreatedAt = _clock.Now(),
                ExpiresOn = _clock.Now().AddMinutes(15),
                Token = Const.SecondActivationToken
            }
        };

        await dbContext.Users.AddRangeAsync(users);
        await dbContext.ActivationTokens.AddRangeAsync(activationTokens);

        await dbContext.SaveChangesAsync();
    }

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
        s.AddScoped<IClock, TestClock>();
    };

    #endregion
}