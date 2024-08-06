using Microsoft.EntityFrameworkCore;
using Rapido.Framework.Common.Abstractions.Commands;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Exceptions;
using Rapido.Services.Notifications.Core.Facades;

namespace Rapido.Services.Notifications.Core.Commands.Handlers;

internal sealed class SendEmailHandler : ICommandHandler<SendEmail>
{
    private readonly NotificationsDbContext _dbContext;
    private readonly IEmailSenderFacade _emailSender;

    public SendEmailHandler(NotificationsDbContext dbContext, IEmailSenderFacade emailSender)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
    }
    
    public async Task HandleAsync(SendEmail command)
    {
        var (id, templateName) = command;
        var recipient = await _dbContext.Recipients.SingleOrDefaultAsync(x => x.Id == id);

        if (recipient is null)
        {
            throw new RecipientNotFoundException(id);
        }

        var template = await _dbContext.Templates.SingleOrDefaultAsync(x => x.Name == templateName);

        if (template is null)
        {
            throw new TemplateNotFoundException(templateName);
        }

        await _emailSender.SendEmail(recipient.Email, template);
    }
}