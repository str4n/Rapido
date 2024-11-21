using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Exceptions;
using Rapido.Services.Notifications.Core.Facades;
using Rapido.Services.Notifications.Core.Services;

namespace Rapido.Services.Notifications.Core.Commands.Handlers;

internal sealed class SendEmailVerificationHandler(
    NotificationsDbContext dbContext,
    IEmailSenderFacade emailSender,
    ITemplateService templateService)
    : ICommandHandler<SendEmailVerification>
{
    public async Task HandleAsync(SendEmailVerification command)
    {
        var (id, verificationToken) = command;
        var recipient = await dbContext.Recipients.SingleOrDefaultAsync(x => x.Id == id);

        if (recipient is null)
        {
            throw new RecipientNotFoundException(id);
        }

        var template = await templateService.GetEmailVerificationTemplate(verificationToken);

        await emailSender.SendEmail(recipient.Email, template);
    }
}