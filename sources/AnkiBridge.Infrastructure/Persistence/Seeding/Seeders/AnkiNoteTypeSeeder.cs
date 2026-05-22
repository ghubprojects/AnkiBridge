using AnkiBridge.Domain.Aggregates.AnkiIntegration.NoteType;
using AnkiBridge.Infrastructure.Persistence.Abstractions;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Seeding.DTO;
using AnkiBridge.Infrastructure.Persistence.Seeding.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class AnkiNoteTypeSeeder : IDbSeeder
{
    public int Order => 3;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var seeds = SeedDataLoader.GetSeedDataFromResource<AnkiNoteTypeSeedDTO>();

        foreach (var seed in seeds)
        {
            var exists = await context.AnkiNoteTypes
                .IgnoreQueryFilters()
                .AnyAsync(x => x.Name == seed.Name, cancellationToken);

            if (exists)
                continue;

            var noteType = AnkiNoteType.Create(seed.Name, seed.ExternalId);
            await context.AnkiNoteTypes.AddAsync(noteType, cancellationToken);
        }
    }
}