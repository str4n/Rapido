using Rapido.Services.Notifications.Core.Models;

namespace Rapido.Services.Notifications.Core.Services;

public interface ITemplateService
{
    Task<EmailTemplate> GetUserActivationTemplate(string activationToken);
}