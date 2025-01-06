using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Messaging.Brokers;
using Rapido.Messages.Events;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Exceptions;
using Rapido.Services.Notifications.Core.Facades;
using Rapido.Services.Notifications.Core.Services;

namespace Rapido.Services.Notifications.Core.Messages.Events.External.Handlers;

internal sealed class FundsAddedConsumer(
    NotificationsDbContext dbContext, IEmailSenderFacade emailSender, 
    ITemplateService templateService) : IConsumer<FundsAdded>
{
    public async Task Consume(ConsumeContext<FundsAdded> context)
    {
        var message = context.Message;
        var recipient = await dbContext.Recipients.SingleOrDefaultAsync(x => x.Id == message.OwnerId);

        if (recipient is null)
        {
            throw new RecipientNotFoundException(message.OwnerId);
        }

        var template = await templateService.GetFundsAddedTemplate(message.TransactionId, message.Currency, message.Amount, message.Date);

        await emailSender.SendEmail(recipient.Email, template);
    }
}