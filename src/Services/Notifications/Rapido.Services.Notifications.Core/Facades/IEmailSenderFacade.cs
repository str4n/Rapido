using Rapido.Services.Notifications.Core.Templates;

namespace Rapido.Services.Notifications.Core.Facades;

internal interface IEmailSenderFacade
{
    Task SendEmail(string emailAddress, EmailTemplate template);
}