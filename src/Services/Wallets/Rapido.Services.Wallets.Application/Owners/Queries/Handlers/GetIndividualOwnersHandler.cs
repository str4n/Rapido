using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Owners.DTO;
using Rapido.Services.Wallets.Application.Owners.Mappings;
using Rapido.Services.Wallets.Domain.Owners.Repositories;

namespace Rapido.Services.Wallets.Application.Owners.Queries.Handlers;

internal sealed class GetIndividualOwnersHandler(IIndividualOwnerRepository repository)
    : IQueryHandler<GetIndividualOwners, IEnumerable<IndividualOwnerDto>>
{
    public async Task<IEnumerable<IndividualOwnerDto>> HandleAsync(GetIndividualOwners query, CancellationToken cancellationToken = default)
        => (await repository.GetAllAsync(cancellationToken)).Select(x => x.AsDto());
}