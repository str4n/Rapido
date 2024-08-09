using Microsoft.EntityFrameworkCore;
using Rapido.Services.Urls.Core.Entities;

namespace Rapido.Services.Urls.Core.EF;

internal sealed class UrlsDbContext : DbContext
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    public UrlsDbContext(DbContextOptions<UrlsDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}