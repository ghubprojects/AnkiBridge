using AnkiBridge.Domain.Aggregates.AnkiIntegration.Deck;
using AnkiBridge.Infrastructure.Persistence.Abstractions;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Seeding.DTO;
using AnkiBridge.Infrastructure.Persistence.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class AnkiDeckSeeder : IDbSeeder
{
    public int Order => 4;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var seeds = SeedDataLoader.GetSeedDataFromResource<AnkiDeckSeedDTO>();

        foreach (var seed in seeds)
        {
            var exists = await context.AnkiDecks
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Name == seed.Name, cancellationToken);

            if (exists)
                continue;

            var deck = AnkiDeck.Create(seed.Name, seed.ExternalId);
            await context.AnkiDecks.AddAsync(deck, cancellationToken);
        }
    }
}