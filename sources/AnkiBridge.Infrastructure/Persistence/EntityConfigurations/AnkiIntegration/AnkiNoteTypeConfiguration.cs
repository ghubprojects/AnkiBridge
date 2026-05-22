using AnkiBridge.Domain.Aggregates.AnkiIntegration.NoteType;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnkiBridge.Infrastructure.Persistence.EntityConfigurations.AnkiIntegration;

public sealed class AnkiNoteTypeConfiguration : IEntityTypeConfiguration<AnkiNoteType>
{
    public void Configure(EntityTypeBuilder<AnkiNoteType> builder)
    {
        // Table
        builder.ToTable("AnkiNoteType", "AnkiIntegration");

        // Primary key
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        // Core
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ExternalId);

        // Audit
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.LastModifiedAt);

        builder.Property(x => x.LastModifiedBy);

        // Soft Delete
        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DeletedAt);

        builder.Property(x => x.DeletedBy);

        builder.HasQueryFilter(x => !x.IsDeleted);

        // Indexes
        builder.HasIndex(x => x.ExternalId);

        // Domain events
        builder.Ignore(x => x.DomainEvents);
    }
}
