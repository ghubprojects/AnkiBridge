using LexiBridge.Domain.Aggregates.Dictionary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LexiBridge.Infrastructure.Persistence.EntityConfigurations.Dictionary;

public sealed class EntryDefinitionConfiguration : IEntityTypeConfiguration<EntryDefinition>
{
    public void Configure(EntityTypeBuilder<EntryDefinition> builder)
    {
        // Table name
        builder.ToTable("EntryDefinition", "Dictionary");

        // Primary key
        builder.HasKey(x => x.Id);

        // Foreign key
        builder.Property<Guid>("DictionaryEntryId")
            .IsRequired();

        // Content
        builder.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.OrderIndex)
            .IsRequired();

        // Examples
        builder.HasMany(x => x.Examples)
            .WithOne()
            .HasForeignKey("DefinitionId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Examples)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Indexes
        builder.HasIndex("DictionaryEntryId");
    }
}
