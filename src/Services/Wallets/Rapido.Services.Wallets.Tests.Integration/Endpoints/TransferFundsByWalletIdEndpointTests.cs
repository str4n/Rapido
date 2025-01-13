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
using Rapido.Services.Wallets.Domain.Wallets.Transfer;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;
using Rapido.Services.Wallets.Infrastructure.EF;
using Rapido.Services.Wallets.Tests.Unit;

namespace Rapido.Services.Wallets.Tests.Integration.Endpoints;

public class TransferFundsByWalletIdEndpointTests() 
    : ApiTests<Program, WalletsDbContext>(options => new WalletsDbContext(options), new ApiTestOptions
    {
        DefaultHttpClientHeaders = new()
        {
            {"Authorization", $"Bearer {Jwt}" }
        }
    })
{
    private Task<HttpResponseMessage> Act(TransferFundsByWalletId command) 
        => Client.PostAsJsonAsync("/transfer/id", command);

    [Fact]
    public async Task given_valid_transfer_funds_request_should_succeed()
    {
        var senderWalletId = Guid.Parse(Const.Wallet1Id);
        var receiverWalletId = Guid.Parse(Const.Wallet2Id);
        var currency = new Currency("EUR");
        var amount = new Amount(20);
        var command = new TransferFundsByWalletId(
                default, 
                senderWalletId, 
                receiverWalletId, 
                "name", 
                currency, 
                amount);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var senderWallet = await TestDbContext.Wallets
            .Include(x => x.Transfers)
            .Include(x => x.Balances)
            .ThenInclude(x => x.Transfers)
            .SingleAsync(x => x.Id == new WalletId(senderWalletId));
        
        var receiverWallet = await TestDbContext.Wallets
            .Include(x => x.Transfers)
            .Include(x => x.Balances)
            .ThenInclude(x => x.Transfers)
            .SingleAsync(x => x.Id == new WalletId(receiverWalletId));

        senderWallet.Balances.First().Amount.Should().Be(Amount.Zero);
        receiverWallet.Balances.First().Amount.Should().Be(amount);
    }
    
    #region Arrange
    private static readonly string Jwt = new TestAuthenticator()
        .GenerateJwt(Guid.Parse(Const.Owner1Id), "admin", "email@test.com");

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
        
        var owner1Id = Guid.Parse(Const.Owner1Id);
        var owner2Id = Guid.Parse(Const.Owner2Id);

        var owners = new IndividualOwner[]
        {
            new(owner1Id, "name1", "fullname1", clock.Now()),
            new(owner2Id, "name2", "fullname2", clock.Now())
        };

        await dbContext.IndividualOwners.AddRangeAsync(owners);

        var currency = new Currency("EUR");

        var senderWallet = new Wallet(Guid.Parse(Const.Wallet1Id), owner1Id, currency, clock.Now());
        var receiverWallet = new Wallet(Guid.Parse(Const.Wallet2Id), owner2Id, currency, clock.Now());

        senderWallet.AddFunds(TransactionId.Create(),"name", 20, currency, TestExchangeRates.GetExchangeRates(), clock.Now());

        await dbContext.Wallets.AddAsync(senderWallet);
        await dbContext.Wallets.AddAsync(receiverWallet);

        await dbContext.SaveChangesAsync();
    }
    
    #endregion
}