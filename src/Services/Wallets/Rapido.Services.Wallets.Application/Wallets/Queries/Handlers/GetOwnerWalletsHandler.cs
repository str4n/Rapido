using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Wallets.DTO;
using Rapido.Services.Wallets.Application.Wallets.Mappings;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;

namespace Rapido.Services.Wallets.Application.Wallets.Queries.Handlers;

internal sealed class GetOwnerWalletsHandler : IQueryHandler<GetOwnerWallets, IEnumerable<WalletDto>>
{
    private readonly IWalletRepository _walletRepository;

    public GetOwnerWalletsHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<IEnumerable<WalletDto>> HandleAsync(GetOwnerWallets query)
        => (await _walletRepository.GetAllAsync(query.OwnerId,false)).Select(x => x.AsDto());
}