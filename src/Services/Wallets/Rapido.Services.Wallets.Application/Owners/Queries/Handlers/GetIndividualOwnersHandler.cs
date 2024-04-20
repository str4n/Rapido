using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Owners.DTO;
using Rapido.Services.Wallets.Application.Owners.Mappings;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Queries.Handlers;

internal sealed class GetIndividualOwnersHandler : IQueryHandler<GetIndividualOwners, IEnumerable<IndividualOwnerDto>>
{
    private readonly IIndividualOwnerRepository _repository;

    public GetIndividualOwnersHandler(IIndividualOwnerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<IndividualOwnerDto>> HandleAsync(GetIndividualOwners query)
        => (await _repository.GetAllAsync()).Select(x => x.AsDto());
}