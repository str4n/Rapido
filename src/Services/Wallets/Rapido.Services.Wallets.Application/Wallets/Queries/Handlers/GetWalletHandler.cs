using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Wallets.DTO;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Application.Wallets.Mappings;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;

namespace Rapido.Services.Wallets.Application.Wallets.Queries.Handlers;

internal sealed class GetWalletHandler : IQueryHandler<GetWallet, WalletDto>
{
    private readonly IWalletRepository _walletRepository;

    public GetWalletHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<WalletDto> HandleAsync(GetWallet query)
    { 
        var wallet = await _walletRepository.GetAsync(new OwnerId(query.OwnerId));

        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }

        return wallet.AsDto();
    }
}