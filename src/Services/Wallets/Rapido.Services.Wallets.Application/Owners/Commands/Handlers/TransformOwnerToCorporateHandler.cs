using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Framework.Common.Time;
using Rapido.Services.Wallets.Application.Owners.Exceptions;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Commands.Handlers;

internal sealed class TransformOwnerToCorporateHandler : ICommandHandler<TransformOwnerToCorporate>
{
    private readonly IIndividualOwnerRepository _individualOwnerRepository;
    private readonly ICorporateOwnerRepository _corporateOwnerRepository;

    public TransformOwnerToCorporateHandler(IIndividualOwnerRepository individualOwnerRepository, 
        ICorporateOwnerRepository corporateOwnerRepository)
    {
        _individualOwnerRepository = individualOwnerRepository;
        _corporateOwnerRepository = corporateOwnerRepository;
    }
    
    public async Task HandleAsync(TransformOwnerToCorporate command)
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
    }
}