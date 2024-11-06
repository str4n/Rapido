using MassTransit;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Events;

internal sealed class CustomerLockedConsumer : IConsumer<CustomerLocked>
{
    private readonly IOwnerRepository _repository;

    public CustomerLockedConsumer(IOwnerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Consume(ConsumeContext<CustomerLocked> context)
    {
        var message = context.Message;

        var owner = await _repository.GetAsync(message.CustomerId);

        if (owner is null)
        {
            return;
        }

        if (owner.State is OwnerState.Locked)
        {
            return;
        }
        
        owner.Lock();

        await _repository.UpdateAsync(owner);
    }
}