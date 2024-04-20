using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Owners.DTO;
using Rapido.Services.Wallets.Application.Owners.Mappings;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Queries.Handlers;

internal sealed class GetCorporateOwnersHandler : IQueryHandler<GetCorporateOwners, IEnumerable<CorporateOwnerDto>>
{
    private readonly ICorporateOwnerRepository _repository;

    public GetCorporateOwnersHandler(ICorporateOwnerRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<CorporateOwnerDto>> HandleAsync(GetCorporateOwners query)
        => (await _repository.GetAllAsync()).Select(x => x.AsDto());
}