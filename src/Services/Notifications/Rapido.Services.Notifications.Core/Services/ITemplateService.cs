using Rapido.Services.Notifications.Core.Templates;

namespace Rapido.Services.Notifications.Core.Services;

internal interface ITemplateService
{
    Task<EmailTemplate> GetUserActivationTemplate(string activationToken);
    Task<EmailTemplate> GetFundsAddedTemplate(string transactionId, string currency, double amount, DateTime transferDate);
    Task<EmailTemplate> GetFundsDeductedTemplate(string transactionId, string currency, double amount, DateTime transferDate);
    Task<EmailTemplate> GetPasswordRecoveryTemplate(string recoveryToken);
}