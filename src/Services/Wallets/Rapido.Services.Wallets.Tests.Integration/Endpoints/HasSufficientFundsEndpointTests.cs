using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Wallets.Application.Wallets.Clients;
using Rapido.Services.Wallets.Application.Wallets.DTO;
using Rapido.Services.Wallets.Application.Wallets.Queries;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;
using Rapido.Services.Wallets.Infrastructure.EF;
using Rapido.Services.Wallets.Tests.Unit;

namespace Rapido.Services.Wallets.Tests.Integration.Endpoints;

public class HasSufficientFundsEndpointTests() 
    : ApiTests<Program, WalletsDbContext>(options => new WalletsDbContext(options))
{
    private Task<HttpResponseMessage> Act(double amount, string currency) 
        => Client.GetAsync($"/has-sufficient-funds?amount={amount}&currency={currency}");

    [Fact]
    public async Task wallet_with_sufficient_funds_request_should_return_true()
    {
        var response = await Act(20, "EUR");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<SufficientFundsDto>();

        content.HasSufficientFunds.Should().BeTrue();
    }
    
    [Theory]
    [InlineData(21, "EUR")]
    [InlineData(25, "USD")]
    [InlineData(17, "GBP")]
    public async Task wallet_with_insufficient_funds_request_should_return_false(double amount, string currency)
    {
        var response = await Act(amount, currency);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<SufficientFundsDto>();

        content.HasSufficientFunds.Should().BeFalse();
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

        wallet.AddFunds(TransactionId.Create(),"test", 20, currency, TestExchangeRates.GetExchangeRates(), clock.Now());

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