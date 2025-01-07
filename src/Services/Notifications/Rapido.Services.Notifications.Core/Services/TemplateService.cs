using Microsoft.EntityFrameworkCore;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Exceptions;
using Rapido.Services.Notifications.Core.Templates;
using Rapido.Services.Notifications.Core.Templates.Models;
using Razor.Templating.Core;

namespace Rapido.Services.Notifications.Core.Services;

internal sealed class TemplateService(NotificationsDbContext dbContext) : ITemplateService
{
    public async Task<EmailTemplate> GetUserActivationTemplate(string activationToken)
        => await GetTemplate(new ActivateUserTemplateModel(activationToken), EmailTemplate.ActivationEmailTemplateName);

    public async Task<EmailTemplate> GetFundsAddedTemplate(string transactionId, string currency, double amount,
        DateTime transferDate)
        => await GetTemplate(new FundsAddedTemplateModel(transactionId, currency, amount, transferDate),
            EmailTemplate.FundsAddedTemplateName);

    public async Task<EmailTemplate> GetFundsDeductedTemplate(string transactionId, string currency, double amount,
        DateTime transferDate)
        => await GetTemplate(new FundsDeductedTemplateModel(transactionId, currency, amount, transferDate),
            EmailTemplate.FundsDeductedTemplateName);

    public async Task<EmailTemplate> GetPasswordRecoveryTemplate(string recoveryToken)
        => await GetTemplate(new PasswordRecoveryTemplateModel(recoveryToken),
            EmailTemplate.PasswordRecoveryTemplateName);

    private async Task<EmailTemplate> GetTemplate<TModel>(TModel model, string templateName) where TModel : TemplateModel
    {
        var templateEntity = await dbContext.Templates.SingleOrDefaultAsync(x => x.Name == templateName);

        if (templateEntity is null)
        {
            throw new TemplateNotFoundException(templateName);
        }

        var subject = templateEntity.Subject;
        var body = await RazorTemplateEngine
            .RenderAsync($"{templateEntity.TemplatePath}", model);

        return new EmailTemplate(subject, body);
    }
}