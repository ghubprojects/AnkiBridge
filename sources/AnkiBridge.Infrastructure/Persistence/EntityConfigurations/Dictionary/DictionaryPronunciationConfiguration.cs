using AnkiBridge.Domain.Aggregates.Dictionary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnkiBridge.Infrastructure.Persistence.EntityConfigurations.Dictionary;

public sealed class DictionaryPronunciationConfiguration : IEntityTypeConfiguration<DictionaryPronunciation>
{
    public void Configure(EntityTypeBuilder<DictionaryPronunciation> builder)
    {
        // Table name
        builder.ToTable("DictionaryPronunciation", "Dictionary");

        // Primary key
        builder.HasKey(p => p.Id);

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
            .HasMaxLength(500);

        builder.Property(x => x.AudioSource)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}
