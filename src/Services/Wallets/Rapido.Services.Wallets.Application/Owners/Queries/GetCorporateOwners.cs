using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Owners.DTO;
using Rapido.Services.Wallets.Domain.Owners.Owner;

namespace Rapido.Services.Wallets.Application.Owners.Queries;

public sealed record GetCorporateOwners : IQuery<IEnumerable<CorporateOwnerDto>>;