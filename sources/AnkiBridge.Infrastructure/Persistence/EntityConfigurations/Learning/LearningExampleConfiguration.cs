using AnkiBridge.Domain.Aggregates.Learning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnkiBridge.Infrastructure.Persistence.EntityConfigurations.Learning;

public sealed class LearningExampleConfiguration : IEntityTypeConfiguration<LearningExample>
{
    public void Configure(EntityTypeBuilder<LearningExample> builder)
    {
        builder.ToTable("LearningExample", "Learning");

        builder.HasKey(example => example.Id);

        builder.Property<Guid>("LearningEntryId")
            .IsRequired();
        builder.Property(example => example.Text)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(example => example.OrderIndex)
            .IsRequired();

        builder.HasIndex("LearningEntryId");
    }
}
