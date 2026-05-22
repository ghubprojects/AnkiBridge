using AnkiBridge.Domain.Aggregates.AnkiIntegration.Note;
using AnkiBridge.Infrastructure.Persistence.Abstractions;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Seeding.DTO;
using AnkiBridge.Infrastructure.Persistence.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class AnkiNoteSeeder : IDbSeeder
{
    public int Order => 5;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var seeds = SeedDataLoader.GetSeedDataFromResource<AnkiNoteSeedDTO>();

        foreach (var seed in seeds)
        {
            var deck = await context.AnkiDecks
                .FirstOrDefaultAsync(x => x.Name == seed.DeckName, cancellationToken);

            if (deck is null)
                continue;

            var noteType = await context.AnkiNoteTypes
                .FirstOrDefaultAsync(x => x.Name == seed.NoteTypeName, cancellationToken);

            if (noteType is null)
                continue;

            var learningEntryId = seed.LearningEntryId
                ?? await context.LearningEntries.Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);

            if (learningEntryId == Guid.Empty)
                continue;

            var exists = await context.AnkiNotes
                .IgnoreQueryFilters()
                .AnyAsync(x =>
                    x.LearningEntryId == learningEntryId
                    && x.DeckId == deck.Id
                    && x.NoteTypeId == noteType.Id,
                    cancellationToken);

            if (exists)
                continue;

            var note = AnkiNote.Create(learningEntryId, noteType.Id, deck.Id);
            await context.AnkiNotes.AddAsync(note, cancellationToken);
        }
    }
}