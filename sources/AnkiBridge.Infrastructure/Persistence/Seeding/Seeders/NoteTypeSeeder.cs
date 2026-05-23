using AnkiBridge.Domain.Aggregates.Flashcard.NoteTypes;
using AnkiBridge.Infrastructure.Persistence.Abstractions;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Seeding.Helpers;
using AnkiBridge.Infrastructure.Persistence.Seeding.Models;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class NoteTypeSeeder : IDbSeeder
{
    public int Order => 3;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var seeds = SeedDataLoader.GetSeedDataFromResource<NoteTypeSeed>();

        foreach (var seed in seeds)
        {
            var exists = await context.NoteTypes
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Name == seed.Name, cancellationToken);

            if (exists)
                continue;

            var noteType = NoteType.Create(seed.Name, seed.ExternalId);
            await context.NoteTypes.AddAsync(noteType, cancellationToken);
        }
    }
}