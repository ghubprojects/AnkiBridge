using LexiBridge.Domain.Aggregates.Exporting.Decks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LexiBridge.Infrastructure.Persistence.EntityConfigurations.Exporting;

public sealed class DeckConfiguration : IEntityTypeConfiguration<Deck>
{
    public void Configure(EntityTypeBuilder<Deck> builder)
    {
        // Table
        builder.ToTable("Deck", "Exporting");

        // Primary key
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        // Core
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

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
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.CreatedAt);

        // Domain events
        builder.Ignore(x => x.DomainEvents);
    }
}
