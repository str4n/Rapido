using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Urls.Core.Entities;

namespace Rapido.Services.Urls.Core.EF.Configs;

internal sealed class ShortenedUrlConfiguration : IEntityTypeConfiguration<ShortenedUrl>
{
    public void Configure(EntityTypeBuilder<ShortenedUrl> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Alias).IsUnique();

        builder.Property(x => x.Alias).IsRequired();

        builder.Property(x => x.ShortUrl).IsRequired();

        builder.Property(x => x.LongUrl).IsRequired();

        builder.Property(x => x.ShortUrl).IsRequired();
    }
}