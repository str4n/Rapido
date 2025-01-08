using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Commands;
using Rapido.Messages.Events;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;
using Rapido.Services.Wallets.Infrastructure.EF;

namespace Rapido.Services.Wallets.Application.Owners.Messages.Commands.Handlers;

internal sealed class CreateCorporateOwnerConsumer(WalletsDbContext dbContext, 
    IClock clock, IMessageBroker messageBroker) 
    : IConsumer<CreateCorporateOwner>
{
    public async Task Consume(ConsumeContext<CreateCorporateOwner> context)
    {
        var message = context.Message;

        var owner = new CorporateOwner(message.CustomerId, message.Name, message.TaxId, clock.Now());

        await dbContext.CorporateOwners.AddAsync(owner);
        await dbContext.SaveChangesAsync();
        await messageBroker.PublishAsync(new OwnerCreated(message.CustomerId, message.Nationality));
    }
}