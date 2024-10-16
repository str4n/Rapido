using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rapido.Services.Notifications.Core.Entities;

namespace Rapido.Services.Notifications.Core.EF.Configs;

internal sealed class TemplateConfiguration : IEntityTypeConfiguration<Template>
{
    public void Configure(EntityTypeBuilder<Template> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Name).IsUnique();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Body).IsRequired();
    }
}