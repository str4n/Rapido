using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rapido.Framework.Postgres.Initializers;
using Rapido.Services.Notifications.Core.Entities;
using Rapido.Services.Notifications.Core.Templates;

namespace Rapido.Services.Notifications.Core.EF;

internal sealed class NotificationsDataInitializer : IDataInitializer
{
    private readonly NotificationsDbContext _dbContext;
    private readonly ILogger<NotificationsDataInitializer> _logger;

    public NotificationsDataInitializer(NotificationsDbContext dbContext, ILogger<NotificationsDataInitializer> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task InitAsync()
    {
        if (await _dbContext.Templates.AnyAsync())
        {
            return;
        }

        await AddTemplatesAsync();
        await _dbContext.SaveChangesAsync();
    }

    private async Task AddTemplatesAsync()
    {
        var templates = new Template[]
        {
            new(
                Guid.NewGuid(),
                EmailTemplate.ActivationEmailTemplateName,
                "Email verification for Rapido",
                $"Templates/{EmailTemplate.ActivationEmailTemplateName}.cshtml"),

            new(
                Guid.NewGuid(),
                EmailTemplate.FundsAddedTemplateName,
                "Funds received on Rapido",
                $"Templates/{EmailTemplate.FundsAddedTemplateName}.cshtml"),
            
            new(
                Guid.NewGuid(),
                EmailTemplate.FundsDeductedTemplateName,
                "Funds sent on Rapido",
                $"Templates/{EmailTemplate.FundsDeductedTemplateName}.cshtml"),
            
            new(
                Guid.NewGuid(),
                EmailTemplate.PasswordRecoveryTemplateName,
                "Password recovery for Rapido",
                $"Templates/{EmailTemplate.PasswordRecoveryTemplateName}.cshtml")
        };
        
        await _dbContext.Templates.AddRangeAsync(templates);
        
        _logger.LogInformation("Templates initialized.");
    }
}