using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Wallets.Application.Wallets.Commands;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;
using Rapido.Services.Wallets.Infrastructure.EF;

namespace Rapido.Services.Wallets.Tests.Integration.Endpoints;

public class AddBalanceEndpointTests() 
    : ApiTests<Program, WalletsDbContext>(options => new WalletsDbContext(options), new ApiTestOptions
{
    DefaultHttpClientHeaders = new()
    {
        {"Authorization", $"Bearer {Jwt}" }
    }
})
{
    private Task<HttpResponseMessage> Act(AddBalance command) 
        => Client.PostAsJsonAsync("/add-balance", command);
    
    [Fact]
    public async Task given_valid_add_balance_request_should_succeed()
    {
        var id = Guid.Parse(Const.Owner1Id);
        var currency = new Currency("PLN");
        var command = new AddBalance(id, currency);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var wallet = await TestDbContext.Wallets
            .Include(x => x.Balances)
            .SingleOrDefaultAsync(x => x.OwnerId == new OwnerId(id));

        wallet.Balances.Count().Should().Be(2);

        var balance = wallet.Balances.SingleOrDefault(x => x.Currency == currency);

        balance.Should().NotBeNull();
        balance?.IsPrimary.Should().BeFalse();
    }
    
    [Fact]
    public async Task add_existing_balance_request_should_return_bad_request_status_code()
    {
        var id = Guid.Parse(Const.Owner1Id);
        var currency = new Currency("EUR");
        var command = new AddBalance(id, currency);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    #region Arrange
    private static readonly string Jwt = new TestAuthenticator()
        .GenerateJwt(Guid.Parse(Const.Owner1Id), "user", "email@test.com");

    protected override Action<IServiceCollection> ConfigureServices => s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };

    protected override async Task SeedAsync()
    {
        var dbContext = TestDbContext;
        await dbContext.Database.MigrateAsync();

        var clock = new TestClock();
        var ownerId = Guid.Parse(Const.Owner1Id);

        var owner = new IndividualOwner(ownerId, "name", "fullname", clock.Now());

        await dbContext.IndividualOwners.AddAsync(owner);

        var wallet = Wallet.Create(ownerId, new Currency("EUR"), clock.Now());

        await dbContext.Wallets.AddAsync(wallet);

        await dbContext.SaveChangesAsync();
    }

    #endregion
}