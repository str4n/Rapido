using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Wallets.Clients;
using Rapido.Services.Wallets.Application.Wallets.DTO;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Application.Wallets.Mappings;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;

namespace Rapido.Services.Wallets.Application.Wallets.Queries.Handlers;

internal sealed class GetWalletHandler(IWalletRepository walletRepository, ICurrencyApiClient client)
    : IQueryHandler<GetWallet, WalletDto>
{
    public async Task<WalletDto> HandleAsync(GetWallet query, CancellationToken cancellationToken = default)
    { 
        var wallet = await walletRepository.GetAsync(new OwnerId(query.OwnerId));

        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }

        var exchangeRates = await client.GetExchangeRates();

        return wallet.AsDto(exchangeRates.ToList());
    }
}