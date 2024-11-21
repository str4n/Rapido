using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rapido.Framework.Postgres.Initializers;
using Rapido.Services.Notifications.Core.Entities;

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
        await _dbContext.Templates
            .AddAsync(new Template(Guid.NewGuid(), "verify_email", "Email verification for Rapido", "Templates/VerifyEmail.cshtml"));
        
        _logger.LogInformation("Templates initialized.");
    }
}