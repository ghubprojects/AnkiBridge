using AnkiBridge.Domain.Aggregates.Learning;
using AnkiBridge.Infrastructure.Persistence.Abstractions;
using AnkiBridge.Infrastructure.Persistence.DatabaseContext;
using AnkiBridge.Infrastructure.Persistence.Seeding.DTO;
using AnkiBridge.Infrastructure.Persistence.Seeding.Helpers;

namespace AnkiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class LearningSeeder : IDbSeeder
{
    public int Order => 2;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var seeds = SeedDataLoader.GetSeedDataFromResource<LearningEntrySeedDTO>();

        foreach (var seed in seeds)
        {
            var learningEntry = LearningEntry.Create(
                seed.Headword,
                seed.PartOfSpeech,
                seed.Ipa,
                seed.Accent,
                seed.Cloze,
                seed.Definition,
                seed.Translation,
                seed.Examples,
                seed.AudioPath,
                seed.ImagePath,
                seed.DictionaryEntryId);

            await context.LearningEntries.AddAsync(learningEntry, cancellationToken);
        }
    }
}
