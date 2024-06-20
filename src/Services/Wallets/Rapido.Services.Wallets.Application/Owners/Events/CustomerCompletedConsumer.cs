using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Contracts.Wallets.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Events;

internal sealed class CustomerCompletedConsumer : IConsumer<CustomerCompleted>
{
    private readonly IIndividualOwnerRepository _ownerRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public CustomerCompletedConsumer(IIndividualOwnerRepository ownerRepository, IClock clock, IMessageBroker messageBroker)
    {
        _ownerRepository = ownerRepository;
        _clock = clock;
        _messageBroker = messageBroker;
    }
    
    public async Task Consume(ConsumeContext<CustomerCompleted> context)
    {
        var message = context.Message;

        var owner = new IndividualOwner(message.CustomerId, message.Name, message.FullName, _clock.Now());

        await _ownerRepository.AddAsync(owner);
        await _messageBroker.PublishAsync(new OwnerCreated(message.CustomerId, message.Nationality));
    }
}