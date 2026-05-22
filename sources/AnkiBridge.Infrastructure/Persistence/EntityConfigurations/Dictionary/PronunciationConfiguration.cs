using AnkiBridge.Domain.Aggregates.Dictionary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnkiBridge.Infrastructure.Persistence.EntityConfigurations.Dictionary;

public sealed class PronunciationConfiguration : IEntityTypeConfiguration<Pronunciation>
{
    public void Configure(EntityTypeBuilder<Pronunciation> builder)
    {
        // Table name
        builder.ToTable("Pronunciation", "Dictionary");

        // Primary key
        builder.HasKey(x => x.Id);

        // Foreign key
        builder.Property<Guid>("DictionaryEntryId")
            .IsRequired();

        // Content
        builder.Property(x => x.Ipa)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Accent)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(x => x.AudioUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.AudioSource)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        // Indexes
        builder.HasIndex("DictionaryEntryId");
        builder.HasIndex(x => new { x.Ipa, x.Accent });
    }
}
