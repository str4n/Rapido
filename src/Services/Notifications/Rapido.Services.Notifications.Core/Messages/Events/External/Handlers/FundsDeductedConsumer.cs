using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rapido.Messages.Events;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Exceptions;
using Rapido.Services.Notifications.Core.Facades;
using Rapido.Services.Notifications.Core.Services;

namespace Rapido.Services.Notifications.Core.Messages.Events.External.Handlers;

internal sealed class FundsDeductedConsumer(
    NotificationsDbContext dbContext, IEmailSenderFacade emailSender, 
    ITemplateService templateService) : IConsumer<FundsDeducted>
{
    public async Task Consume(ConsumeContext<FundsDeducted> context)
    {
        var message = context.Message;
        var recipient = await dbContext.Recipients.SingleOrDefaultAsync(x => x.Id == message.OwnerId);

        if (recipient is null)
        {
            throw new RecipientNotFoundException(message.OwnerId);
        }

        var template = await templateService.GetFundsDeductedTemplate(message.TransactionId, message.Currency, message.Amount, message.Date);

        await emailSender.SendEmail(recipient.Email, template);
    }
}