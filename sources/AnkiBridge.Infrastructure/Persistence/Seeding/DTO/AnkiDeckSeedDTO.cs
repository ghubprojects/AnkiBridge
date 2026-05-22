namespace AnkiBridge.Infrastructure.Persistence.Seeding.DTO;

internal sealed record AnkiDeckSeedDTO
{
    public required string Name { get; init; }
    public long ExternalId { get; init; }
}