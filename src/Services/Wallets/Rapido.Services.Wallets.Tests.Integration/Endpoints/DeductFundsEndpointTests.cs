using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Wallets.Application.Wallets.Clients;
using Rapido.Services.Wallets.Application.Wallets.Commands;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;
using Rapido.Services.Wallets.Infrastructure.EF;

namespace Rapido.Services.Wallets.Tests.Integration.Endpoints;

public class DeductFundsEndpointTests()
    : ApiTests<Program, WalletsDbContext>(options => new WalletsDbContext(options))
{
    private Task<HttpResponseMessage> Act(DeductFunds command) 
        => Client.PutAsJsonAsync("/deduct-funds", command);
    
    [Fact]
    public async Task given_valid_deduct_funds_request_should_succeed()
    {
        var id = Guid.Parse(Const.Wallet1Id);
        var amount = 20;
        var command = new DeductFunds(id, new Currency("EUR"), amount);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var wallet = await TestDbContext.Wallets
            .Include(x => x.Balances)
            .SingleOrDefaultAsync(x => x.Id == new WalletId(id));

        var balance = wallet.Balances.Single();
        
        balance.Amount.Value.Should().Be(Amount.Zero);
    }
    
    [Fact]
    public async Task given_valid_deduct_funds_request_wallet_with_insufficient_funds_should_return_bad_request_status_code()
    {
        var id = Guid.Parse(Const.Wallet1Id);
        var amount = 30;
        var command = new DeductFunds(id, new Currency("EUR"), amount);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var wallet = await TestDbContext.Wallets
            .Include(x => x.Balances)
            .SingleOrDefaultAsync(x => x.Id == new WalletId(id));

        var balance = wallet.Balances.Single();
        
        balance.Amount.Value.Should().Be(20);
    }
    
    
    
    #region Arrange

    protected override Action<IServiceCollection> ConfigureServices => s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
        s.AddScoped<ICurrencyApiClient, TestCurrencyApiClient>();
    };

    protected override async Task SeedAsync()
    {
        var dbContext = TestDbContext;
        await dbContext.Database.MigrateAsync();

        var clock = new TestClock();
        
        var ownerId = Guid.Parse(Const.Owner1Id);

        var owner = new IndividualOwner(ownerId, "name", "fullname", clock.Now());

        await dbContext.IndividualOwners.AddAsync(owner);

        var currency = new Currency("EUR");
        var wallet = new Wallet(Guid.Parse(Const.Wallet1Id), ownerId, currency, clock.Now());

        wallet.AddFunds("test", 20, currency, new ExchangeRate(currency, currency, 1), clock.Now());

        await dbContext.Wallets.AddAsync(wallet);

        await dbContext.SaveChangesAsync();
    }

    protected override void AddClientHeaders()
    {
        var jwt = new TestAuthenticator()
            .GenerateJwt(Guid.Parse(Const.Owner1Id), "admin", "email@test.com");
        
        Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");
    }

    #endregion
}