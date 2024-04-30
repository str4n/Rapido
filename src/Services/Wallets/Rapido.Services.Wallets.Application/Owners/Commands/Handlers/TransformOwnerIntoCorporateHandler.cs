using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Wallets.Events;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Services.Wallets.Application.Owners.Exceptions;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Commands.Handlers;

internal sealed class TransformOwnerIntoCorporateHandler : ICommandHandler<TransformOwnerIntoCorporate>
{
    private readonly IIndividualOwnerRepository _individualOwnerRepository;
    private readonly ICorporateOwnerRepository _corporateOwnerRepository;
    private readonly IMessageBroker _messageBroker;

    public TransformOwnerIntoCorporateHandler(IIndividualOwnerRepository individualOwnerRepository, 
        ICorporateOwnerRepository corporateOwnerRepository, IMessageBroker messageBroker)
    {
        _individualOwnerRepository = individualOwnerRepository;
        _corporateOwnerRepository = corporateOwnerRepository;
        _messageBroker = messageBroker;
    }
    
    public async Task HandleAsync(TransformOwnerIntoCorporate command)
    {
        var ownerId = command.OwnerId;
        var corporateOwner = await _corporateOwnerRepository.GetAsync(ownerId);

        if (corporateOwner is not null)
        {
            throw new CorporateOwnerAlreadyExists(ownerId);
        }
        
        var individualOwner = await _individualOwnerRepository.GetAsync(ownerId);

        if (individualOwner is null)
        {
            throw new OwnerNotFoundException(ownerId);
        }

        await _individualOwnerRepository.DeleteAsync(individualOwner);

        corporateOwner = individualOwner.TransformToCorporateOwner(command.TaxId);

        await _corporateOwnerRepository.AddAsync(corporateOwner);
        await _messageBroker.PublishAsync(new IndividualOwnerTransformedIntoCorporate(corporateOwner.Id));
    }
}