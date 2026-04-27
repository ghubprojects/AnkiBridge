using LexiBridge.Domain.Aggregates.Exporting.LearningItemExports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LexiBridge.Infrastructure.Persistence.EntityConfigurations.Exporting;

public sealed class LearningItemExportConfiguration : IEntityTypeConfiguration<LearningItemExport>
{
    public void Configure(EntityTypeBuilder<LearningItemExport> builder)
    {
        // Table
        builder.ToTable("LearningItemExport", "Exporting");

        // Primary key
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        // References
        builder.Property(x => x.LearningItemId)
            .IsRequired();

        builder.Property(x => x.DeckId)
            .IsRequired();

        builder.Property(x => x.CardTemplateId)
            .IsRequired();

        // Export state
        builder.Property(x => x.Destination)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.Attempts)
            .IsRequired();

        builder.Property(x => x.ExternalId)
            .HasMaxLength(100);

        builder.Property(x => x.Error)
            .HasMaxLength(1000);

        builder.Property(x => x.ExportedAt);

        // Indexes
        builder.HasIndex(x => x.LearningItemId);
        builder.HasIndex(x => x.DeckId);
        builder.HasIndex(x => x.CardTemplateId);
        builder.HasIndex(x => new { x.Destination, x.Status });

        // Domain events
        builder.Ignore(x => x.DomainEvents);
    }
}
