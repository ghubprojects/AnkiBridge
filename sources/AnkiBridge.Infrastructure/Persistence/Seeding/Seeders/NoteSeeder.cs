using AnkiBridge.Domain.Aggregates.Flashcard.Notes;
using AnkiBridge.Infrastructure.Persistence.Abstractions;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Seeding.Helpers;
using AnkiBridge.Infrastructure.Persistence.Seeding.Models;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class NoteSeeder : IDbSeeder
{
    public int Order => 5;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var seeds = SeedDataLoader.GetSeedDataFromResource<NoteSeed>();

        foreach (var seed in seeds)
        {
            var deck = await context.Decks
                .FirstOrDefaultAsync(x => x.Name == seed.DeckName, cancellationToken);

            if (deck is null)
                continue;

            var noteType = await context.NoteTypes
                .FirstOrDefaultAsync(x => x.Name == seed.NoteTypeName, cancellationToken);

            if (noteType is null)
                continue;

            var learningEntryId = seed.LearningEntryId
                ?? await context.LearningEntries.Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);

            if (learningEntryId == Guid.Empty)
                continue;

            var exists = await context.Notes
                .IgnoreQueryFilters()
                .AnyAsync(x =>
                    x.LearningEntryId == learningEntryId
                    && x.DeckId == deck.Id
                    && x.NoteTypeId == noteType.Id,
                    cancellationToken);

            if (exists)
                continue;

            var note = Note.Create(learningEntryId, noteType.Id, deck.Id);
            await context.Notes.AddAsync(note, cancellationToken);
        }
    }
}