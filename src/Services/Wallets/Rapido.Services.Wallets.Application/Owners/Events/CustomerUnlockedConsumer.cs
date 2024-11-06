using MassTransit;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Events;

internal sealed class CustomerUnlockedConsumer : IConsumer<CustomerUnlocked>
{
    private readonly IOwnerRepository _repository;

    public CustomerUnlockedConsumer(IOwnerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Consume(ConsumeContext<CustomerUnlocked> context)
    {
        var message = context.Message;

        var owner = await _repository.GetAsync(message.CustomerId);

        if (owner is null)
        {
            return;
        }

        if (owner.State is not OwnerState.Locked)
        {
            return;
        }
        
        owner.Unlock();

        await _repository.UpdateAsync(owner);
    }
}