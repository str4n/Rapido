using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rapido.Framework.Common.Time;
using Rapido.Services.Users.Core.EF;

namespace Rapido.Services.Users.Core.Services;

internal sealed class ActivationTokenCleaner(
    IServiceProvider serviceProvider, 
    IClock clock, 
    ILogger<ActivationTokenCleaner> logger) : IHostedService, IDisposable
{
    private Timer _timer;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(6));

        return Task.CompletedTask;
    }
    
    private void DoWork(object state)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
        
        var now = clock.Now();
        
        logger.LogInformation("Cleaning expired activation tokens...");
        
        dbContext.ActivationTokens.Where(x => x.ExpiresOn < now).ExecuteDelete();
        dbContext.SaveChangesAsync();
        
        logger.LogInformation("Expired activation tokens cleared.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    public void Dispose()
        => _timer.Dispose();
}