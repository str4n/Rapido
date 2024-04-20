using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Events;

internal sealed class CustomerCompletedConsumer : IConsumer<CustomerCompleted>
{
    private readonly IIndividualOwnerRepository _ownerRepository;
    private readonly IClock _clock;

    public CustomerCompletedConsumer(IIndividualOwnerRepository ownerRepository, IClock clock)
    {
        _ownerRepository = ownerRepository;
        _clock = clock;
    }
    
    public async Task Consume(ConsumeContext<CustomerCompleted> context)
    {
        var message = context.Message;

        var owner = new IndividualOwner(message.CustomerId, message.Name, message.FullName, _clock.Now());

        await _ownerRepository.AddAsync(owner);
    }
}