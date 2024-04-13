using Microsoft.EntityFrameworkCore;
using Rapido.Services.Customers.Core.Entities.Customer;
using Rapido.Services.Customers.Core.Entities.Lockout;

namespace Rapido.Services.Customers.Core.EF;

internal sealed class CustomersDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Lockout> Lockouts { get; set; }

    public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}