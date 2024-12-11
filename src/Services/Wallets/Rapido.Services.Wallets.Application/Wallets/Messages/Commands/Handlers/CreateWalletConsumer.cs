using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Commands;
using Rapido.Messages.Events;
using Rapido.Services.Wallets.Domain.Wallets.Money;
using Rapido.Services.Wallets.Domain.Wallets.Repositories;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Application.Wallets.Messages.Commands.Handlers;

internal sealed class CreateWalletConsumer(IWalletRepository repository, IClock clock, IMessageBroker messageBroker) 
    : IConsumer<CreateWallet>
{
    public async Task Consume(ConsumeContext<CreateWallet> context)
    {
        var message = context.Message;
        var currency = GetCurrencyBasedOnNationality();

        var wallet = Wallet.Create(message.OwnerId, currency, clock.Now());

        await repository.AddAsync(wallet);

        Currency GetCurrencyBasedOnNationality() => message.Nationality switch
        {
            "PL" => Currency.PLN(),
            "GB" => Currency.GBP(),
            "US" => Currency.USD(),
            _ => Currency.EUR(),
        };

        await messageBroker.PublishAsync(new WalletCreated(wallet.Id, wallet.OwnerId, currency));
    }
}