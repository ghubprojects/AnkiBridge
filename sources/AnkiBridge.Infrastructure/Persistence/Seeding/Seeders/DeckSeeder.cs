using AnkiBridge.Domain.Aggregates.Flashcard.Decks;
using AnkiBridge.Infrastructure.Persistence.Abstractions;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Seeding.Helpers;
using AnkiBridge.Infrastructure.Persistence.Seeding.Models;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class DeckSeeder : IDbSeeder
{
    public int Order => 4;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var seeds = SeedDataLoader.GetSeedDataFromResource<DeckSeed>();

        foreach (var seed in seeds)
        {
            var exists = await context.Decks
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Name == seed.Name, cancellationToken);

            if (exists)
                continue;

            var deck = Deck.Create(seed.Name, seed.ExternalId);
            await context.Decks.AddAsync(deck, cancellationToken);
        }
    }
}