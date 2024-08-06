using Microsoft.EntityFrameworkCore;
using Rapido.Services.Notifications.Core.Entities;

namespace Rapido.Services.Notifications.Core.EF;

internal sealed class NotificationsDbContext : DbContext
{
    public DbSet<Template> Templates { get; set; }
    public DbSet<Recipient> Recipients { get; set; }

    public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}