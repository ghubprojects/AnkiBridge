using AnkiBridge.Domain.Aggregates.Dictionary;
using AnkiBridge.Infrastructure.Persistence.Abstractions;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Seeding.DTO;
using AnkiBridge.Infrastructure.Persistence.Seeding.Helpers;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class DictionarySeeder : IDbSeeder
{
    public int Order => 1;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var seeds = SeedDataLoader.GetSeedDataFromResource<DictionaryEntrySeedDTO>();

        foreach (var seed in seeds)
        {
            var dictionaryEntry = DictionaryEntry.Create(seed.Headword, seed.PartOfSpeech, seed.Source);

            foreach (var pronunciation in seed.Pronunciations)
            {
                dictionaryEntry.AddPronunciation(
                    pronunciation.Ipa,
                    pronunciation.Accent,
                    pronunciation.AudioUrl,
                    pronunciation.AudioSource);
            }

            foreach (var definition in seed.Definitions)
                dictionaryEntry.AddDefinition(definition.Text, definition.Examples);

            foreach (var image in seed.Images)
                dictionaryEntry.AddImage(image.Url, image.Source);

            await context.DictionaryEntries.AddAsync(dictionaryEntry, cancellationToken);
        }
    }
}
