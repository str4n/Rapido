﻿using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Testing;
using Rapido.Framework.Testing.Abstractions;
using Rapido.Services.Customers.Core.Common.Domain.Customer;
using Rapido.Services.Customers.Core.Common.EF;
using Rapido.Services.Customers.Core.Individual.Commands;
using Rapido.Services.Customers.Core.Individual.Domain.Customer;

namespace Rapido.Services.Customers.Tests.Integration.Endpoints;

public class CompleteIndividualCustomerEndpointTests()
    : ApiTests<Program, CustomersDbContext>(options => new CustomersDbContext(options), new ApiTestOptions
    {
        DefaultHttpClientHeaders = new()
        {
            { "Authorization", $"Bearer {Jwt}" }
        }
    })
{
    private Task<HttpResponseMessage> Act(CompleteIndividualCustomer command) 
        => Client.PostAsJsonAsync("/individual/complete", command);
    
    [Fact]
    public async Task given_valid_complete_individual_customer_request_should_succeed()
    {
        var id = Guid.Parse(Const.IndividualCustomerGuid);

        var name = new Name("iname");
        var fullName = new FullName("full name");
        var address = new Address("country", "province", "city", "street", "00-000");
        var nationality = new Nationality("PL");

        var command = new CompleteIndividualCustomer(id, name, fullName, 
            address.Country, address.Province, address.City, address.Street,
            address.PostalCode, nationality);

        var response = await Act(command);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var customer = await TestDbContext.IndividualCustomers.SingleOrDefaultAsync(x => x.Id == id);

        customer.Should().NotBeNull();
        customer.IsCompleted.Should().BeTrue();

        customer.Name.Should().Be(name);
        customer.FullName.Should().Be(fullName);
        customer.Address.Should().Be(address);
        customer.Nationality.Should().Be(nationality);
    }
    
    
    #region Arrange
    
    private static readonly string Jwt = new TestAuthenticator()
        .GenerateJwt(Guid.Parse(Const.IndividualCustomerGuid), "user", Const.IndividualCustomerEmail);

    protected override Action<IServiceCollection> ConfigureServices { get; } = s =>
    {
        s.AddScoped<IMessageBroker, TestMessageBroker>();
    };
    
    protected override async Task SeedAsync()
    {
        var dbContext = TestDbContext;
        await dbContext.Database.MigrateAsync();
        
        var clock = new TestClock();

        await dbContext.IndividualCustomers.AddAsync(new IndividualCustomer(Guid.Parse(Const.IndividualCustomerGuid),
            Const.IndividualCustomerEmail, clock.Now()));

        await dbContext.SaveChangesAsync();
    }
    
    #endregion Arrange
}