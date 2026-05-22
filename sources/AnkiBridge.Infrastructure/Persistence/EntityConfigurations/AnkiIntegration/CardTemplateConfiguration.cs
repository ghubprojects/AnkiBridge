using AnkiBridge.Domain.Aggregates.AnkiIntegration.CardTemplates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnkiBridge.Infrastructure.Persistence.EntityConfigurations.AnkiIntegration;

public sealed class CardTemplateConfiguration : IEntityTypeConfiguration<CardTemplate>
{
    public void Configure(EntityTypeBuilder<CardTemplate> builder)
    {
        // Table
        builder.ToTable("CardTemplate", "Flashcard");

        // Primary key
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        // Core
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.Css)
            .IsRequired()
            .HasMaxLength(4000);

        // Card types
        builder.HasMany(x => x.CardTypes)
            .WithOne()
            .HasForeignKey(x => x.CardTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.CardTypes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

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
