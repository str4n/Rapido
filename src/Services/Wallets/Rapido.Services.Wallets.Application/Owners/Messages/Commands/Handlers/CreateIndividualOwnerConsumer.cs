using MassTransit;
using Rapido.Framework.Common.Time;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Framework.Postgres.UnitOfWork;
using Rapido.Messages.Commands;
using Rapido.Messages.Events;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Owners.Repositories;
using Rapido.Services.Wallets.Infrastructure.EF;

namespace Rapido.Services.Wallets.Application.Owners.Messages.Commands.Handlers;

internal sealed class CreateIndividualOwnerConsumer(
    IIndividualOwnerRepository repository, 
    IUnitOfWork unitOfWork,
    IClock clock, 
    IMessageBroker messageBroker) 
    : IConsumer<CreateIndividualOwner>
{
    public async Task Consume(ConsumeContext<CreateIndividualOwner> context)
    {
        await unitOfWork.ExecuteAsync(async () =>
        {
            var message = context.Message;

            var owner = new IndividualOwner(message.CustomerId, message.Name, message.FullName, clock.Now());

            await repository.AddAsync(owner);
            await messageBroker.PublishAsync(new OwnerCreated(message.CustomerId, message.Nationality));
        });
    }
}