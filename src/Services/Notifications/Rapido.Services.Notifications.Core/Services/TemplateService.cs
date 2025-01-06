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
    {
        var templateName = EmailTemplate.ActivationEmailTemplateName;
        var templateEntity = await dbContext.Templates.SingleOrDefaultAsync(x => x.Name == templateName);

        if (templateEntity is null)
        {
            throw new TemplateNotFoundException(templateName);
        }

        var subject = templateEntity.Subject;
        var body = await RazorTemplateEngine
            .RenderAsync($"{templateEntity.TemplatePath}", new ActivateUserTemplateModel(activationToken));

        return new EmailTemplate(subject, body);
    }

    public async Task<EmailTemplate> GetFundsAddedTemplate(string transactionId, string currency, double amount, DateTime transferDate)
    {
        var templateName = EmailTemplate.FundsAddedTemplateName;
        var templateEntity = await dbContext.Templates.SingleOrDefaultAsync(x => x.Name == templateName);

        if (templateEntity is null)
        {
            throw new TemplateNotFoundException(templateName);
        }

        var subject = templateEntity.Subject;
        var body = await RazorTemplateEngine
            .RenderAsync($"{templateEntity.TemplatePath}", new FundsAddedTemplateModel(transactionId, currency, amount, transferDate));

        return new EmailTemplate(subject, body);
    }

    public async Task<EmailTemplate> GetFundsDeductedTemplate(string transactionId, string currency, double amount, DateTime transferDate)
    {
        var templateName = EmailTemplate.FundsDeductedTemplateName;
        var templateEntity = await dbContext.Templates.SingleOrDefaultAsync(x => x.Name == templateName);

        if (templateEntity is null)
        {
            throw new TemplateNotFoundException(templateName);
        }

        var subject = templateEntity.Subject;
        var body = await RazorTemplateEngine
            .RenderAsync($"{templateEntity.TemplatePath}", new FundsDeductedTemplateModel(transactionId, currency, amount, transferDate));

        return new EmailTemplate(subject, body);
    }
}