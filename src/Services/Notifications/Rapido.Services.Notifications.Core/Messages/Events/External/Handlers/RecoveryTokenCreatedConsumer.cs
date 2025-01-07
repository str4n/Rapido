using MassTransit;
using Microsoft.EntityFrameworkCore;
using Rapido.Messages.Events;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Exceptions;
using Rapido.Services.Notifications.Core.Facades;
using Rapido.Services.Notifications.Core.Services;

namespace Rapido.Services.Notifications.Core.Messages.Events.External.Handlers;

internal sealed class RecoveryTokenCreatedConsumer(
    NotificationsDbContext dbContext, 
    IEmailSenderFacade emailSender,
    ITemplateService templateService) : IConsumer<RecoveryTokenCreated>
{
    public async Task Consume(ConsumeContext<RecoveryTokenCreated> context)
    {
        var message = context.Message;
        var recipient = await dbContext.Recipients.SingleOrDefaultAsync(x => x.Id == message.UserId);

        if (recipient is null)
        {
            throw new RecipientNotFoundException(message.UserId);
        }

        var template = await templateService.GetPasswordRecoveryTemplate(message.Token);
        
        await emailSender.SendEmail(recipient.Email, template);
    }
}