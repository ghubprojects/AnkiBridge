using LexiBridge.Domain.Aggregates.Dictionary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LexiBridge.Infrastructure.Persistence.EntityConfigurations.Dictionary;

public sealed class EntryImageConfiguration : IEntityTypeConfiguration<EntryImage>
{
    public void Configure(EntityTypeBuilder<EntryImage> builder)
    {
        // Table name
        builder.ToTable("EntryImage", "Dictionary");

        // Primary key
        builder.HasKey(x => x.Id);

        // Foreign key
        builder.Property<Guid>("DictionaryEntryId")
            .IsRequired();

        // Content
        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Source)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        // Indexes
        builder.HasIndex("DictionaryEntryId");
    }
}
