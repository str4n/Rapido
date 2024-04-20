using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Owners.DTO;

namespace Rapido.Services.Wallets.Application.Owners.Queries;

public sealed record GetIndividualOwners : IQuery<IEnumerable<IndividualOwnerDto>>;