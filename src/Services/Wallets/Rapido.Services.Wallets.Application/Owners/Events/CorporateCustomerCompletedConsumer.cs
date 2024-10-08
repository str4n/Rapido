using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Contracts.Wallets.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Events;

internal sealed class CorporateCustomerCompletedConsumer : IConsumer<CorporateCustomerCompleted>
{
    private readonly ICorporateOwnerRepository _ownerRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;

    public CorporateCustomerCompletedConsumer(ICorporateOwnerRepository ownerRepository, IClock clock, IMessageBroker messageBroker)
    {
        _ownerRepository = ownerRepository;
        _clock = clock;
        _messageBroker = messageBroker;
    }
    
    public async Task Consume(ConsumeContext<CorporateCustomerCompleted> context)
    {
        var message = context.Message;

        var owner = new CorporateOwner(message.CustomerId, message.Name, message.TaxId, _clock.Now());

        await _ownerRepository.AddAsync(owner);
        await _messageBroker.PublishAsync(new CorporateOwnerCreated(message.CustomerId, message.Nationality));
    }
}