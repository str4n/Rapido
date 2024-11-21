using Rapido.Services.Notifications.Core.Entities;
using Rapido.Services.Notifications.Core.Models;

namespace Rapido.Services.Notifications.Core.Facades;

internal interface IEmailSenderFacade
{
    Task SendEmail(string emailAddress, EmailTemplate template);
}