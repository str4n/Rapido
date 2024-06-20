using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Wallets.DTO;

namespace Rapido.Services.Wallets.Application.Wallets.Queries;

public sealed record GetWallet(Guid OwnerId) : IQuery<WalletDto>;