using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Services.Wallets.Application.Owners.Exceptions;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Events;

internal sealed class CustomerVerifiedConsumer : IConsumer<CustomerVerified>
{
    private readonly IIndividualOwnerRepository _ownerRepository;
    private readonly IClock _clock;

    public CustomerVerifiedConsumer(IIndividualOwnerRepository ownerRepository, IClock clock)
    {
        _ownerRepository = ownerRepository;
        _clock = clock;
    }
    
    public async Task Consume(ConsumeContext<CustomerVerified> context)
    {
        var message = context.Message;

        var owner = await _ownerRepository.GetAsync(message.CustomerId);

        if (owner is null)
        {
            throw new OwnerNotFoundException(message.CustomerId);
        }
        
        owner.Verify(_clock.Now());
        await _ownerRepository.UpdateAsync(owner);
    }
}