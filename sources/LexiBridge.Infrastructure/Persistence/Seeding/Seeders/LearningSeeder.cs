using LexiBridge.Domain.Aggregates.Learning;
using LexiBridge.Infrastructure.Persistence.Abstractions;
using LexiBridge.Infrastructure.Persistence.Contexts;
using LexiBridge.Infrastructure.Persistence.Seeding.DTO;
using LexiBridge.Infrastructure.Persistence.Seeding.Helpers;

namespace LexiBridge.Infrastructure.Persistence.Seeding.Seeders;

public sealed class LearningSeeder : IDbSeeder
{
    public int Order => 2;

    public async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var learningItemSeeds = SeedDataLoader.GetSeedDataFromResource<LearningItemSeedDTO>();

        foreach (var seed in learningItemSeeds)
        {
            var learningItem = LearningItem.Create(
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

            await context.LearningItems.AddAsync(learningItem, cancellationToken);
        }
    }
}
