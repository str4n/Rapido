using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Contracts.Wallets.Events;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Application.Wallets.Events;

internal sealed class OwnerCreatedConsumer : IConsumer<OwnerCreated>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IClock _clock;

    public OwnerCreatedConsumer(IWalletRepository walletRepository, IClock clock)
    {
        _walletRepository = walletRepository;
        _clock = clock;
    }
    
    public async Task Consume(ConsumeContext<OwnerCreated> context)
    {
        var message = context.Message;

        var wallet = Wallet.Create(message.OwnerId, GetCurrencyBasedOnNationality(), _clock.Now());

        await _walletRepository.AddAsync(wallet);

        Currency GetCurrencyBasedOnNationality() => message.Nationality switch
        {
            "PL" => Currency.PLN(),
            "GB" => Currency.GBP(),
            "US" => Currency.USD(),
            _ => Currency.EUR(),
        };
    }
}