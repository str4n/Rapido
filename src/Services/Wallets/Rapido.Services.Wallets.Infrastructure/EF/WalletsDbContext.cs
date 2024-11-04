using Microsoft.EntityFrameworkCore;
using Rapido.Services.Wallets.Domain.Owners.Owner;
using Rapido.Services.Wallets.Domain.Wallets.Wallet;

namespace Rapido.Services.Wallets.Infrastructure.EF;

internal sealed class WalletsDbContext : DbContext
{
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<IndividualOwner> IndividualOwners { get; set; }
    public DbSet<CorporateOwner> CorporateOwners { get; set; }
    public DbSet<Owner> Owners { get; set; }

    public WalletsDbContext(DbContextOptions<WalletsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}