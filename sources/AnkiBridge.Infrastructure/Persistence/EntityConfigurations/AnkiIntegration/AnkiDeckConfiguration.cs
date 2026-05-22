using AnkiBridge.Domain.Aggregates.AnkiIntegration.Deck;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnkiBridge.Infrastructure.Persistence.EntityConfigurations.AnkiIntegration;

public sealed class AnkiDeckConfiguration : IEntityTypeConfiguration<AnkiDeck>
{
    public void Configure(EntityTypeBuilder<AnkiDeck> builder)
    {
        // Table
        builder.ToTable("AnkiDeck", "AnkiIntegration");

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
