using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Owners.DTO;
using Rapido.Services.Wallets.Application.Owners.Mappings;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Queries.Handlers;

internal sealed class GetCorporateOwnersHandler(ICorporateOwnerRepository repository)
    : IQueryHandler<GetCorporateOwners, IEnumerable<CorporateOwnerDto>>
{
    public async Task<IEnumerable<CorporateOwnerDto>> HandleAsync(GetCorporateOwners query, CancellationToken cancellationToken = default)
        => (await repository.GetAllAsync(cancellationToken)).Select(x => x.AsDto());
}