using LexiBridge.Domain.Aggregates.Dictionary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LexiBridge.Infrastructure.Persistence.EntityConfigurations.Dictionary;

public sealed class EntryExampleConfiguration : IEntityTypeConfiguration<EntryExample>
{
    public void Configure(EntityTypeBuilder<EntryExample> builder)
    {
        // Table name
        builder.ToTable("EntryExample", "Dictionary");

        // Primary key
        builder.HasKey(x => x.Id);

        // Foreign key
        builder.Property<Guid>("DefinitionId")
            .IsRequired();

        // Content
        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(200);

        // Indexes
        builder.HasIndex("DefinitionId");
    }
}
