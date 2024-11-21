using FluentEmail.Core;
using Rapido.Services.Notifications.Core.Entities;
using Rapido.Services.Notifications.Core.Models;

namespace Rapido.Services.Notifications.Core.Facades;

internal sealed class EmailSenderFacade(IFluentEmail fluentEmailService) : IEmailSenderFacade
{
    public async Task SendEmail(string emailAddress, EmailTemplate template)
        => await fluentEmailService
            .To(emailAddress)
            .Subject(template.Subject)
            .Body(template.Body).SendAsync();
}