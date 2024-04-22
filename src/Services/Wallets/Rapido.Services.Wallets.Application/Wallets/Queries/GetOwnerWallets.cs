using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Wallets.DTO;

namespace Rapido.Services.Wallets.Application.Wallets.Queries;

public sealed record GetOwnerWallets(Guid OwnerId) : IQuery<IEnumerable<WalletDto>>;