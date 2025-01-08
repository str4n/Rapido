using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Wallets.Clients;
using Rapido.Services.Wallets.Application.Wallets.DTO;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;

namespace Rapido.Services.Wallets.Application.Wallets.Queries.Handlers;

internal sealed class CheckSufficiencyOfFundsHandler(IWalletRepository walletRepository, ICurrencyApiClient client)
    : IQueryHandler<CheckSufficiencyOfFunds, SufficientFundsDto>
{
    public async Task<SufficientFundsDto> HandleAsync(CheckSufficiencyOfFunds query, CancellationToken cancellationToken = default)
    {
        var ownerId = new OwnerId(query.OwnerId);
        var amount = new Amount(query.Amount);
        var currency = new Currency(query.Currency);

        var wallet = await walletRepository.GetAsync(ownerId);
        
        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }
        
        var exchangeRates = (await client.GetExchangeRates()).ToList();
        
        if (exchangeRates is null || !exchangeRates.Any())
        {
            throw new ExchangeRateNotFoundException();
        }

        var result = wallet.HasSufficientFunds(amount, currency, exchangeRates);

        return new SufficientFundsDto(wallet.Id, result);
    }
}