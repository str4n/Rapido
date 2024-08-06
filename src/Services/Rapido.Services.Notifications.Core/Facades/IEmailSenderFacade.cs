using Rapido.Services.Notifications.Core.Entities;

namespace Rapido.Services.Notifications.Core.Facades;

internal interface IEmailSenderFacade
{
    Task SendEmail(string emailAddress, Template template);
}