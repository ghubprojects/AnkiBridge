using LexiBridge.Domain.Aggregates.Learning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LexiBridge.Infrastructure.Persistence.EntityConfigurations.Learning;

public sealed class LearningExampleConfiguration : IEntityTypeConfiguration<LearningExample>
{
    public void Configure(EntityTypeBuilder<LearningExample> builder)
    {
        // Table name
        builder.ToTable("LearningExample", "Learning");

        // Primary key
        builder.HasKey(x => x.Id);

        // Foreign key
        builder.Property<Guid>("LearningItemId")
            .IsRequired();

        // Content
        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(200);

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
        builder.HasIndex("LearningItemId");
    }
}
