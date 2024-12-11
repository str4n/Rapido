using MassTransit;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Commands;
using Rapido.Messages.Events;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Entities;

namespace Rapido.Services.Notifications.Core.Messages.Commands.Handlers;

internal sealed class CreateRecipientConsumer(NotificationsDbContext dbContext, IMessageBroker messageBroker) 
    : IConsumer<CreateRecipient>
{
    public async Task Consume(ConsumeContext<CreateRecipient> context)
    {
        var message = context.Message;

        var recipient = new Recipient(message.Id, message.Email);

        await dbContext.Recipients.AddAsync(recipient);
        await dbContext.SaveChangesAsync();
        await messageBroker.PublishAsync(new RecipientCreated(recipient.Id));
    }
}