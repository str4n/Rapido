using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Rapido.Framework.Postgres.Initializers;

internal sealed class DatabaseInitializer<T> : IHostedService where T : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer<T>> _logger;

    public DatabaseInitializer(IServiceProvider serviceProvider, ILogger<DatabaseInitializer<T>> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
        
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>() as DbContext;
        
        _logger.LogInformation("Migrating the dbContext: {dbContext}...", dbContext);
        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}