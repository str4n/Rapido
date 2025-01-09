using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Postgres.UnitOfWork;
using Rapido.Messages.Events;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;
using Rapido.Services.Wallets.Infrastructure.EF;

namespace Rapido.Services.Wallets.Application.Owners.Messages.Events.External.Handlers;

internal sealed class CustomerUnlockedConsumer(IOwnerRepository repository, IUnitOfWork unitOfWork) : IConsumer<CustomerUnlocked>
{
    public async Task Consume(ConsumeContext<CustomerUnlocked> context)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            var message = context.Message;

            var owner = await repository.GetAsync(message.CustomerId);

            if (owner is null)
            {
                return;
            }

            if (owner.State is not OwnerState.Locked)
            {
                return;
            }

            owner.Unlock();

            await repository.UpdateAsync(owner);
        });
    }
}