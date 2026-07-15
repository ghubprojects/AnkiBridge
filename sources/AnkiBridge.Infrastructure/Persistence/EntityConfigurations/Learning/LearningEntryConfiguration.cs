using AnkiBridge.Domain.Aggregates.Learning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnkiBridge.Infrastructure.Persistence.EntityConfigurations.Learning;

public sealed class LearningEntryConfiguration : IEntityTypeConfiguration<LearningEntry>
{
    public void Configure(EntityTypeBuilder<LearningEntry> builder)
    {
        builder.ToTable("LearningEntry", "Learning");

        builder.HasKey(entry => entry.Id);

        builder.Property(entry => entry.Headword)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(entry => entry.PartOfSpeech)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);
        builder.Property(entry => entry.Cloze)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(entry => entry.Definition)
            .IsRequired()
            .HasMaxLength(1_000);
        builder.Property(entry => entry.Translation)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(entry => entry.TranslationSource)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);
        builder.Property(entry => entry.Accent)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);
        builder.Property(entry => entry.Ipa)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(entry => entry.AudioSource)
            .HasConversion<string>()
            .HasMaxLength(30);
        builder.Property(entry => entry.AudioPath)
            .HasMaxLength(1_000);
        builder.Property(entry => entry.AudioDownloadStatus)
            .HasConversion<string>()
            .HasMaxLength(20);
        builder.Property(entry => entry.ImageSource)
            .HasConversion<string>()
            .HasMaxLength(30);
        builder.Property(entry => entry.ImagePath)
            .HasMaxLength(1_000);
        builder.Property(entry => entry.ImageDownloadStatus)
            .HasConversion<string>()
            .HasMaxLength(20);
        builder.Property(entry => entry.CreatedAt)
            .IsRequired();
        builder.Property(entry => entry.IsDeleted)
            .IsRequired();

        builder.HasIndex(entry => entry.DictionaryEntryId);
        builder.HasIndex(entry => entry.CreatedAt);

        builder.HasMany(entry => entry.Examples)
            .WithOne()
            .HasForeignKey("LearningEntryId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(entry => entry.Examples)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasQueryFilter(entry => !entry.IsDeleted);
        builder.Ignore(entry => entry.DomainEvents);
    }
}
