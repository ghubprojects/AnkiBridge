using AnkiBridge.Domain.Aggregates.AnkiIntegration.Note;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnkiBridge.Infrastructure.Persistence.EntityConfigurations.AnkiIntegration;

public sealed class AnkiNoteConfiguration : IEntityTypeConfiguration<AnkiNote>
{
    public void Configure(EntityTypeBuilder<AnkiNote> builder)
    {
        // Table
        builder.ToTable("AnkiNote", "AnkiIntegration");

        // Primary key
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        // Foreign keys
        builder.Property(x => x.LearningEntryId)
            .IsRequired();

        builder.Property(x => x.NoteTypeId)
            .IsRequired();

        builder.Property(x => x.DeckId)
            .IsRequired();

        builder.HasOne(x => x.LearningEntry)
            .WithMany()
            .HasForeignKey(x => x.LearningEntryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.NoteType)
            .WithMany()
            .HasForeignKey(x => x.NoteTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Deck)
            .WithMany()
            .HasForeignKey(x => x.DeckId)
            .OnDelete(DeleteBehavior.Restrict);

        // Core
        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.ExternalId);

        builder.Property(x => x.ExportedAt);

        // Audit
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.LastModifiedAt);

        builder.Property(x => x.LastModifiedBy);

        // Soft delete
        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DeletedAt);

        builder.Property(x => x.DeletedBy);

        builder.HasQueryFilter(x => !x.IsDeleted);

        // Indexes
        builder.HasIndex(x => new { x.LearningEntryId, x.NoteTypeId, x.DeckId })
            .IsUnique();

        // Domain events
        builder.Ignore(x => x.DomainEvents);
    }
}