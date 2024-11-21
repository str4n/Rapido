using Microsoft.EntityFrameworkCore;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Exceptions;
using Rapido.Services.Notifications.Core.Models;
using Rapido.Services.Notifications.Core.TemplateModels;
using Razor.Templating.Core;

namespace Rapido.Services.Notifications.Core.Services;

internal sealed class TemplateService(NotificationsDbContext dbContext) : ITemplateService
{
    private const string TemplateName = "verify_email";

    public async Task<EmailTemplate> GetEmailVerificationTemplate(string verificationToken)
    {
        var templateEntity = await dbContext.Templates.SingleOrDefaultAsync(x => x.Name == TemplateName);

        if (templateEntity is null)
        {
            throw new TemplateNotFoundException(TemplateName);
        }

        var title = templateEntity.Subject;
        var body = await RazorTemplateEngine
            .RenderAsync($"{templateEntity.TemplatePath}", new VerifyEmailTemplateModel(verificationToken));

        return new EmailTemplate(title, body);
    }
}