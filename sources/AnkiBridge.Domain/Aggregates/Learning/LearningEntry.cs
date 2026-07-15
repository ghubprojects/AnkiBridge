using AnkiBridge.Domain.Enums;
using AnkiBridge.Domain.SeedWork;
using AnkiBridge.Shared.Results;

namespace AnkiBridge.Domain.Aggregates.Learning;

public sealed class LearningEntry : AggregateRoot<Guid>, ISoftDeleteEntity
{
    public string Headword { get; private set; } = default!;
    public PartOfSpeech PartOfSpeech { get; private set; }
    public string Cloze { get; private set; } = default!;
    public string Definition { get; private set; } = default!;
    public string Translation { get; private set; } = default!;
    public TranslationSource TranslationSource { get; private set; }
    public Accent Accent { get; private set; }
    public string Ipa { get; private set; } = default!;
    public AudioSource? AudioSource { get; private set; }
    public string? AudioPath { get; private set; }
    public MediaDownloadStatus? AudioDownloadStatus { get; private set; }
    public ImageSource? ImageSource { get; private set; }
    public string? ImagePath { get; private set; }
    public MediaDownloadStatus? ImageDownloadStatus { get; private set; }
    public Guid? DictionaryEntryId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public Guid? DeletedBy { get; private set; }

    private readonly List<LearningExample> _examples = [];
    public IReadOnlyCollection<LearningExample> Examples => _examples.AsReadOnly();

    private LearningEntry() { }

    private LearningEntry(
        string headword,
        PartOfSpeech partOfSpeech,
        string cloze,
        string definition,
        string translation,
        TranslationSource translationSource,
        Accent accent,
        string ipa,
        Guid? dictionaryEntryId)
    {
        Id = Guid.CreateVersion7();
        Headword = headword;
        PartOfSpeech = partOfSpeech;
        Cloze = cloze;
        Definition = definition;
        Translation = translation;
        TranslationSource = translationSource;
        Accent = accent;
        Ipa = ipa;
        DictionaryEntryId = dictionaryEntryId;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public static Result<LearningEntry> Create(
        string headword,
        PartOfSpeech partOfSpeech,
        string cloze,
        string definition,
        string translation,
        TranslationSource translationSource,
        Accent accent,
        string ipa,
        Guid? dictionaryEntryId,
        IReadOnlyList<string>? examples = null)
    {
        if (string.IsNullOrWhiteSpace(headword))
            return Result.Failure<LearningEntry>("Headword must not be empty.");

        if (string.IsNullOrWhiteSpace(cloze))
            return Result.Failure<LearningEntry>("Cloze must not be empty.");

        if (string.IsNullOrWhiteSpace(definition))
            return Result.Failure<LearningEntry>("Definition must not be empty.");

        if (string.IsNullOrWhiteSpace(translation))
            return Result.Failure<LearningEntry>("Translation must not be empty.");

        if (string.IsNullOrWhiteSpace(ipa))
            return Result.Failure<LearningEntry>("IPA must not be empty.");

        var entry = new LearningEntry(
            headword.Trim(),
            partOfSpeech,
            cloze.Trim(),
            definition.Trim(),
            translation.Trim(),
            translationSource,
            accent,
            ipa.Trim(),
            dictionaryEntryId);

        foreach (var example in examples?
                     .Where(example => !string.IsNullOrWhiteSpace(example))
                     .Take(3) ?? [])
        {
            var result = entry.AddExample(example);
            if (result.IsFailure)
                return Result.Failure<LearningEntry>(result.Error, result.ErrorType);
        }

        return entry;
    }

    public Result AddExample(string text)
    {
        if (_examples.Count >= 3)
            return Result.Failure("A learning entry can have at most three examples.");

        var result = LearningExample.Create(text, _examples.Count + 1);
        if (result.IsFailure)
            return result;

        _examples.Add(result.Value);
        return Result.Success();
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
    }

    public static string GenerateCloze(string headword)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(headword);

        var characters = headword.Trim().ToCharArray();
        var consonantIndexes = new List<int>();

        for (var index = 0; index < characters.Length; index++)
        {
            if (!char.IsLetter(characters[index]))
                continue;

            if ("aeiouAEIOU".Contains(characters[index]))
            {
                characters[index] = '_';
                continue;
            }

            consonantIndexes.Add(index);
        }

        var additionalMasks = consonantIndexes.Count == 0
            ? 0
            : Random.Shared.Next(1, Math.Min(3, consonantIndexes.Count) + 1);

        foreach (var index in consonantIndexes.OrderBy(_ => Random.Shared.Next()).Take(additionalMasks))
            characters[index] = '_';

        return new string(characters);
    }
}
