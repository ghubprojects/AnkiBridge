using LexiBridge.Domain.Aggregates.Exporting.CardTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LexiBridge.Infrastructure.Persistence.EntityConfigurations.Exporting;

public sealed class CardTypeConfiguration : IEntityTypeConfiguration<CardType>
{
    public void Configure(EntityTypeBuilder<CardType> builder)
    {
        // Table
        builder.ToTable("CardType", "Exporting");

        // Primary key
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        // Reference
        builder.Property(x => x.CardTemplateId)
            .IsRequired();

        // Core
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.FrontHtml)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(x => x.BackHtml)
            .IsRequired()
            .HasMaxLength(4000);

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
        builder.HasIndex(x => x.CardTemplateId);
        builder.HasIndex(x => new { x.CardTemplateId, x.Name });
    }
}
