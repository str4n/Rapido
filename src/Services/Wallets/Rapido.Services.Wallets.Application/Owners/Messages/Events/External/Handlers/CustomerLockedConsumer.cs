using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rapido.Messages.Events;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;
using Rapido.Services.Wallets.Infrastructure.EF;

namespace Rapido.Services.Wallets.Application.Owners.Messages.Events.External.Handlers;

internal sealed class CustomerLockedConsumer(WalletsDbContext dbContext) : IConsumer<CustomerLocked>
{
    public async Task Consume(ConsumeContext<CustomerLocked> context)
    {
        var message = context.Message;

        var owner = await dbContext.Owners.SingleOrDefaultAsync(x => x.Id == new OwnerId(message.CustomerId));

        if (owner is null)
        {
            return;
        }

        if (owner.State is OwnerState.Locked)
        {
            return;
        }
        
        owner.Lock();

        dbContext.Owners.Update(owner);
        await dbContext.SaveChangesAsync();
    }
}