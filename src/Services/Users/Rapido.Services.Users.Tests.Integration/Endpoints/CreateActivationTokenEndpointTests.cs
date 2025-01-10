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
using Rapido.Messages.Commands;
using Rapido.Services.Users.Core.Shared.EF;
using Rapido.Services.Users.Core.User.Domain;
using Rapido.Services.Users.Core.User.Services;
using Xunit;

namespace Rapido.Services.Users.Tests.Integration.Endpoints;

public class CreateActivationTokenEndpointTests() : ApiTests<Program, UsersDbContext>(options => new UsersDbContext(options))
{
    private Task<HttpResponseMessage> Act(CreateActivationToken command) 
        => Client.PutAsJsonAsync("/create-activation-token", command);

    [Fact]
    public async Task given_valid_create_activation_token_request_should_succeed()
    {
        var response = await Act(new CreateActivationToken(Const.FirstEmail));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var token = await TestDbContext.ActivationTokens.SingleOrDefaultAsync(x =>
            x.UserId == Guid.Parse(Const.UserId));

        token.Should().NotBeNull();
        token.CreatedAt.Should().Be(_clock.Now());
    }
    
    [Fact]
    public async Task already_activated_user_request_should_return_bad_request_status_code()
    {
        var response = await Act(new CreateActivationToken(Const.SecondEmail));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task invalid_user_email_request_should_return_not_found_status_code()
    {
        var response = await Act(new CreateActivationToken("invalid@email.com"));

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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

        var firstUserId = Guid.Parse(Const.UserId);
        var secondUserId = Guid.NewGuid();

        var users = new List<User>
        {
            new()
            {
                Id = firstUserId,
                Email = Const.FirstEmail,
                Password = password,
                IsActivated = false,
                IsDeleted = false,
                CreatedAt = _clock.Now(),
                Role = role
            },
            new()
            {
                Id = secondUserId,
                Email = Const.SecondEmail,
                Password = password,
                IsActivated = true,
                IsDeleted = false,
                CreatedAt = _clock.Now(),
                Role = role
            }
        };
        
        await dbContext.Users.AddRangeAsync(users);

        await dbContext.SaveChangesAsync();
    }

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
        s.AddScoped<IClock, TestClock>();
    };

    #endregion
}