using LexiBridge.Domain.Aggregates.Dictionary;
using LexiBridge.Infrastructure.Persistence.Abstractions;
using LexiBridge.Infrastructure.Persistence.Contexts;
using LexiBridge.Infrastructure.Persistence.Seeding.DTO;
using LexiBridge.Infrastructure.Persistence.Seeding.Helpers;

namespace LexiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class DictionarySeeder : IDbSeeder
{
    public int Order => 1;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var dictionaryEntrySeeds = SeedDataLoader.GetSeedDataFromResource<DictionaryEntrySeedDTO>();

        foreach (var seed in dictionaryEntrySeeds)
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
