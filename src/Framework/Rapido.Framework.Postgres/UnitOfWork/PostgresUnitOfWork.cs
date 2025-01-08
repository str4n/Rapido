using Microsoft.EntityFrameworkCore;

namespace Rapido.Framework.Postgres.UnitOfWork;

internal sealed class PostgresUnitOfWork<TContext>(TContext dbContext) : IUnitOfWork where TContext : DbContext
{
    public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await action();
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }
}