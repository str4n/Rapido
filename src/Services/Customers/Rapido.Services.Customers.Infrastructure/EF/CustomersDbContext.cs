using Microsoft.EntityFrameworkCore;
using Rapido.Services.Customers.Domain.Common.Customer;
using Rapido.Services.Customers.Domain.Common.Lockout;
using Rapido.Services.Customers.Domain.Corporate.Customer;
using Rapido.Services.Customers.Domain.Individual.Customer;

namespace Rapido.Services.Customers.Infrastructure.EF;

internal sealed class CustomersDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<IndividualCustomer> IndividualCustomers { get; set; }
    public DbSet<CorporateCustomer> CorporateCustomers { get; set; }
    public DbSet<Lockout> Lockouts { get; set; }

    public CustomersDbContext(DbContextOptions<CustomersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}