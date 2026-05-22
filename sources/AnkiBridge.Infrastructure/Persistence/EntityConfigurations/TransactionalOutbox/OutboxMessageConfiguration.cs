using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AnkiBridge.Application.Abstractions.TransactionalOutbox;

namespace AnkiBridge.Infrastructure.Persistence.EntityConfigurations.TransactionalOutbox;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        // Table name
        builder.ToTable("OutboxMessages", "TransactionalOutbox");

        // Primary key
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.CreationDate)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(x => x.Payload)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(x => x.PayloadType)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.ProcessedDate)
            .HasColumnType("timestamptz");

        builder.Property(x => x.ProcessedCount)
            .HasDefaultValue(0)
            .IsRequired();

        // Index để query message chưa xử lý nhanh hơn
        builder.HasIndex(x => new { x.ProcessedDate, x.CreationDate });

        // Optional: index cho retry logic
        builder.HasIndex(x => x.ProcessedCount);
    }
}