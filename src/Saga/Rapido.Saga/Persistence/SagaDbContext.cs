using Microsoft.EntityFrameworkCore;
using Rapido.Saga.Sagas;

namespace Rapido.Saga.Persistence;

public sealed class SagaDbContext : DbContext
{
    public DbSet<AccountSetUpSagaData> AccountSetUpSagaData { get; set; }

    public SagaDbContext(DbContextOptions<SagaDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountSetUpSagaData>().HasKey(x => x.CorrelationId);
    }
}