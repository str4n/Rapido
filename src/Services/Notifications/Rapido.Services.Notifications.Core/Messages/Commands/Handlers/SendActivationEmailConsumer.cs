using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Commands;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Exceptions;
using Rapido.Services.Notifications.Core.Facades;
using Rapido.Services.Notifications.Core.Messages.Events;
using Rapido.Services.Notifications.Core.Services;

namespace Rapido.Services.Notifications.Core.Messages.Commands.Handlers;

internal sealed class SendActivationEmailConsumer(
    NotificationsDbContext dbContext, IEmailSenderFacade emailSender, 
    ITemplateService templateService, IMessageBroker messageBroker)
    : IConsumer<SendActivationEmail>
{
    public async Task Consume(ConsumeContext<SendActivationEmail> context)
    {
        var message = context.Message;
        var (id, activationToken) = message;
        var recipient = await dbContext.Recipients.SingleOrDefaultAsync(x => x.Id == id);

        if (recipient is null)
        {
            throw new RecipientNotFoundException(id);
        }

        var template = await templateService.GetUserActivationTemplate(activationToken);

        await emailSender.SendEmail(recipient.Email, template);
        await messageBroker.PublishAsync(new ActivationEmailSent(id, activationToken));
    }
}