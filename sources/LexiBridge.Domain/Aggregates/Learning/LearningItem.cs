using LexiBridge.Domain.Enums;
using LexiBridge.Domain.SeedWork;
using LexiBridge.Shared.Results;

namespace LexiBridge.Domain.Aggregates.Learning;

public sealed class LearningItem : AggregateRoot<Guid>, IAuditableEntity, ISoftDeleteEntity
{
    // References
    public Guid? DictionaryEntryId { get; private set; }

    // Core vocabulary
    public string Headword { get; private set; } = default!;
    public PartOfSpeech PartOfSpeech { get; private set; }
    public string Ipa { get; private set; } = default!;
    public Accent Accent { get; private set; }

    // Learning content
    public string Cloze { get; private set; } = default!;
    public string Definition { get; private set; } = default!;
    public string Translation { get; private set; } = default!;

    // Media
    public string? AudioPath { get; private set; }
    public string? ImagePath { get; private set; }

    // Examples
    private readonly List<LearningExample> _examples = [];
    public IReadOnlyCollection<LearningExample> Examples => _examples.AsReadOnly();

    #region Audit

    public DateTimeOffset CreatedAt { get; }
    public Guid CreatedBy { get; }
    public DateTimeOffset? LastModifiedAt { get; }
    public Guid? LastModifiedBy { get; }

    #endregion

    #region Soft Delete

    public bool IsDeleted { get; }
    public DateTimeOffset? DeletedAt { get; }
    public Guid? DeletedBy { get; }

    #endregion

    private LearningItem() { }

    private LearningItem(
        string headword,
        PartOfSpeech partOfSpeech,
        string ipa,
        Accent accent,
        string cloze,
        string definition,
        string translation,
        IEnumerable<(Guid? Id, string Text)> examples,
        string? audioPath,
        string? imagePath,
        Guid? dictionaryEntryId)
    {
        Id = Guid.CreateVersion7();
        DictionaryEntryId = dictionaryEntryId;

        Headword = headword;
        PartOfSpeech = partOfSpeech;
        Ipa = ipa;
        Accent = accent;

        Cloze = cloze;
        Definition = definition;
        Translation = translation;

        AudioPath = audioPath;
        ImagePath = imagePath;

        UpsertExamples(examples);
    }

    public static LearningItem Create(
        string headword,
        PartOfSpeech partOfSpeech,
        string ipa,
        Accent accent,
        string cloze,
        string definition,
        string translation,
        IEnumerable<string> examples,
        string? audioPath = null,
        string? imagePath = null,
        Guid? dictionaryEntryId = null)
    {
        return new LearningItem(
            headword,
            partOfSpeech,
            ipa,
            accent,
            cloze,
            definition,
            translation,
            examples.Select(x => ((Guid?)null, x)),
            audioPath,
            imagePath,
            dictionaryEntryId);
    }

    public Result Update(
        string headword,
        PartOfSpeech partOfSpeech,
        string ipa,
        Accent accent,
        string cloze,
        string definition,
        string translation,
        IEnumerable<(Guid? Id, string Text)> examples,
        string? audioPath = null,
        string? imagePath = null,
        Guid? dictionaryEntryId = null)
    {
        DictionaryEntryId = dictionaryEntryId;

        Headword = headword;
        PartOfSpeech = partOfSpeech;
        Ipa = ipa;
        Accent = accent;

        Cloze = cloze;
        Definition = definition;
        Translation = translation;

        AudioPath = audioPath;
        ImagePath = imagePath;

        UpsertExamples(examples);

        return Result.Success();
    }

    public void SetAudioPath(string? audioPath)
    {
        AudioPath = audioPath;
    }

    public void SetImagePath(string? imagePath)
    {
        ImagePath = imagePath;
    }

    private void UpsertExamples(IEnumerable<(Guid? Id, string Text)> examples)
    {
        var examplesById = examples
            .Where(ne => ne.Id.HasValue)
            .ToDictionary(ne => ne.Id!.Value, ne => ne.Text);

        // Remove examples that are not in the new list
        _examples.RemoveAll(e => !examplesById.ContainsKey(e.Id));

        // Update existing examples
        foreach (var existing in _examples)
        {
            if (examplesById.TryGetValue(existing.Id, out var newText))
                existing.UpdateText(newText);
        }

        // Add new examples
        var newExamples = examples
            .Where(ne => !ne.Id.HasValue)
            .Select(ne => LearningExample.Create(ne.Text));

        _examples.AddRange(newExamples);
    }
}