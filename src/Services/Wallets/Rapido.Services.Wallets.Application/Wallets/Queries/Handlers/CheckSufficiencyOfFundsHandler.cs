using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Wallets.Application.Wallets.Clients;
using Rapido.Services.Wallets.Application.Wallets.DTO;
using Rapido.Services.Wallets.Application.Wallets.Exceptions;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;

namespace Rapido.Services.Wallets.Application.Wallets.Queries.Handlers;

internal sealed class CheckSufficiencyOfFundsHandler : IQueryHandler<CheckSufficiencyOfFunds, SufficientFundsDto>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ICurrencyApiClient _client;

    public CheckSufficiencyOfFundsHandler(IWalletRepository walletRepository, ICurrencyApiClient client)
    {
        _walletRepository = walletRepository;
        _client = client;
    }

    public async Task<SufficientFundsDto> HandleAsync(CheckSufficiencyOfFunds query)
    {
        var ownerId = new OwnerId(query.OwnerId);
        var amount = new Amount(query.Amount);
        var currency = new Currency(query.Currency);

        var wallet = await _walletRepository.GetAsync(ownerId);
        
        if (wallet is null)
        {
            throw new WalletNotFoundException();
        }
        
        var exchangeRates = (await _client.GetExchangeRates()).ToList();
        
        if (exchangeRates is null || !exchangeRates.Any())
        {
            throw new ExchangeRateNotFoundException();
        }

        var result = wallet.HasSufficientFunds(amount, currency, exchangeRates);

        return new SufficientFundsDto(wallet.Id, result);
    }
}