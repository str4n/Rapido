namespace Rapido.Framework.Postgres.UnitOfWork;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default);
}